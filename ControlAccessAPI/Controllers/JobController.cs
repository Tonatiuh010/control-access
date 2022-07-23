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
public class JobController : CustomContoller 
{
    [HttpGet]
    public Result GetJobs() => RequestResponse(() => "Is empty for now.");

    [HttpGet]
    public Result GetJob(int id) => RequestResponse(() => "Is empty for now");

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

    [HttpPost]
    [Route("SetPosition")]
    public Result SetPosition(dynamic obj) => RequestResponse(() => {
        JObject jObj = JObject.Parse(obj.ToString());

        return bl.SetPosition(
            new Position() {
                Id =  JsonProperty<int?>.GetValue("jobId", jObj, OnMissingProperty),
                Alias = JsonProperty<string>.GetValue("alias", jObj, OnMissingProperty),
                PositionId = JsonProperty<int?>.GetValue("positionId", jObj, OnMissingProperty),
                Departament = new Departament() {
                    Id = JsonProperty<int?>.GetValue("departamentId", jObj, OnMissingProperty)
                }
            },
            C.GLOBAL_USER
        );
    });
}