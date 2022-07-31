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
public class DepartamentController : CustomController
{
    [HttpGet]
    public Result GetDepartaments() => RequestResponse(() => bl.GetDepartaments());

    [HttpGet("{id:int?}")]
    public Result GetDepartament(int? id) => RequestResponse(() => 
        GetItem( bl.GetDepartaments(id) )
    );

    [HttpPost]
    public Result SetDepartament(dynamic obj) => RequestResponse(() => {
        JObject jObj = JObject.Parse(obj.ToString());
        return bl.SetDepartament(new Departament(){
            Id = JsonProperty<int?>.GetValue("id", jObj),
            Name = JsonProperty<string>.GetValue("name", jObj, OnMissingProperty),
            Code = JsonProperty<string>.GetValue("code", jObj, OnMissingProperty)
        }, C.GLOBAL_USER);
    });

}