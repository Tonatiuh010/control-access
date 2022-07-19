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
    private ControlAccessBL bl = new ControlAccessBL();

    [HttpGet]
    public Result Get() => RequestResponse(() => "Empty For now...");

    [HttpPost]
    public Result Set(dynamic obj) => RequestResponse(
        () => bl.SetCheck(
            JsonProperty<string>.GetValue(
                "cardSerial", 
                JObject.Parse(obj.ToString()), 
                OnMissingProperty
            ), 
            C.GLOBAL_USER
        )
    );
       
}