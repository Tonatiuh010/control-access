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
public class ShiftController : CustomController
{    
    [HttpGet]
    public Result GetShifts() => RequestResponse(() => bl.GetShifts());

    [HttpGet("{id:int?}")]
    public Result GetShift(int? id) => RequestResponse(() => 
        GetItem(bl.GetShifts(id)) 
    );

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
            LunchTime = Shift.ConvertTime( JsonProperty<string>.GetValue("lunchTime", jObj, OnMissingProperty) ),
            DayCount = JsonProperty<int?>.GetValue("dayCount", jObj),
        };        

        return bl.SetShift(shift, C.GLOBAL_USER);
    });
}