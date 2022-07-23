using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Newtonsoft.Json.Linq;
using Engine.BL;
using Engine.BL.Delegates;
using Engine.BO;
using Classes;
using Engine.Constants;

namespace ControlAccess.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckController : CustomContoller
{
    [HttpGet]
    public Result Get() => RequestResponse(() => "Empty For now...");

    [HttpGet("employee/{id:int?}")]
    public Result GetWeeklyChecks(int? id) => RequestResponse(() => $"Getting checks of employee ({id})");

    [HttpPost]
    public Result Set(dynamic obj) => RequestResponse(
        () => bl.SetCheck(
            JsonProperty<string>.GetValue(
                "serial", 
                JObject.Parse(obj.ToString()), 
                OnMissingProperty
            ), 
            C.GLOBAL_USER
        )
    );

}