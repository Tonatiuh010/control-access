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
public class LevelController : CustomController
{
    
    [HttpGet]
    public Result GetAccessLevels() => RequestResponse(() => bl.GetAccessLevels());

    [HttpGet("EmployeeLevel/{id:int?}")]
    public Result GetEmployeeAccessLevel(int? id) => RequestResponse(() => bl.GetEmployeeAccessLevels(id));

    [HttpPost]
    public Result SetLevel(dynamic obj) => RequestResponse(() => {
        JObject jObj = JObject.Parse(obj.ToString());
        return bl.SetAccessLevel(
            new AccessLevel() {
                Id = JsonProperty<int?>.GetValue("id", jObj, OnMissingProperty),
                Name = JsonProperty<string>.GetValue("name", jObj, OnMissingProperty)
            }, 
            C.GLOBAL_USER
        );
    });

}