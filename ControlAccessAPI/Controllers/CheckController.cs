using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Newtonsoft.Json.Linq;
using Engine.BL;
using Engine.BL.Delegates;
using Engine.BO;
using Classes;
using Engine.Constants;
using ControlAccess.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ControlAccess.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckController : CustomController
{
    private readonly IHubContext<CheckHub> _hub;

    public CheckController(IHubContext<CheckHub> hub) : base()
    {
        _hub = hub;
        
    }

    [HttpGet]
    public Result Get() => RequestResponse(() => bl.GetChecks());

    [HttpGet("employee/{id:int?}")]
    public Result GetWeeklyChecks(int? id) => RequestResponse(() => $"Getting checks of employee ({id})");

    [HttpGet("{id:int?}")]
    public Result GetCheck(int? id) => RequestResponse(() => 
        GetItem( bl.GetChecks(id) )
    );

    [HttpPost]
    public Result Set(dynamic obj) => RequestResponse(
        () => {
            var serial = JsonProperty<string>.GetValue( "serial", JObject.Parse(obj.ToString()));
            var device = JsonProperty<string>.GetValue("device", JObject.Parse(obj.ToString()));

            ResultInsert result = bl.SetCheck(serial,device, C.GLOBAL_USER);

            if(result != null && result.Status != C.OK)
            {
                var check = GetItem(bl.GetChecks(result.InsertDetails.Id));
                result.Data = check;
                _hub.Clients.All.SendAsync("CheckMonitor", check);
            }

            return result;
        }
    );

}