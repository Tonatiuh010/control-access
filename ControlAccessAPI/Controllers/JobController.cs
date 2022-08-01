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
public class JobController : CustomController
{
    [HttpGet]
    public Result GetJobs() => RequestResponse(() => bl.GetJobs());

    [HttpGet("{id:int?}")]
    public Result GetJob(int? id) => RequestResponse(() => 
        GetItem( bl.GetJobs(id) )
    );

    [HttpPost]
    public Result SetJob(dynamic obj) => RequestResponse(() => {
        JObject jObj = JObject.Parse(obj.ToString());

        return bl.SetJob(
            new Job() {
                Id = JsonProperty<int?>.GetValue("id", jObj),
                Description = JsonProperty<string>.GetValue("description", jObj),
                Name = JsonProperty<string>.GetValue("name", jObj, OnMissingProperty)
            },
            C.GLOBAL_USER
        );
    });

}