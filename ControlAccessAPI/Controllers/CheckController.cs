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

            ResultInsert result = bl.SetCheck(serial, C.GLOBAL_USER );

            if(result != null && result.Status != C.OK)
            {
                result.Data = GetItem(bl.GetChecks(result.InsertDetails.Id));
            }

            return result;
        }
    );

}