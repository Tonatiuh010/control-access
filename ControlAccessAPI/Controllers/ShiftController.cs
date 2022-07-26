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
public class ShiftController : CustomContoller
{    
    [HttpGet]
    public Result GetShift() => RequestResponse(() => "Working on it...");

    [HttpPost]
    public Result SetShift(dynamic obj) => RequestResponse(() => {
        Shift shift;
        
        JObject jObj = JObject.Parse(obj.ToString());        
        shift = new Shift(
            JsonProperty<string>.GetValue("inTime", jObj, OnMissingProperty), 
            JsonProperty<string>.GetValue("outTime", jObj, OnMissingProperty) 
        ) {
            Id = JsonProperty<int?>.GetValue("id", jObj),
            Name = JsonProperty<string>.GetValue("name", jObj, OnMissingProperty),
            DayCount = JsonProperty<int?>.GetValue("dayCount", jObj),            
        };
        

        return bl.SetShift(shift, C.GLOBAL_USER);
    });
}