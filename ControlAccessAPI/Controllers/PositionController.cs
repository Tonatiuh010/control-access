using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Newtonsoft.Json.Linq;
using Engine.BL;
using Engine.BL.Delegates;
using Engine.BO;
using Classes;
using Engine.Constants;


namespace ControlAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : CustomController
    {
        [HttpGet]
        public Result GetPostitions() => RequestResponse(() => bl.GetPositions());

        [HttpGet]
        public Result GetPostitions(dynamic obj) =>  RequestResponse(() => {
            JObject jObj = JObject.Parse(obj.ToString());
            return bl.GetPositions(
                JsonProperty<int?>.GetValue("position", jObj),
                JsonProperty<int?>.GetValue("job", jObj),
                JsonProperty<int?>.GetValue("departament", jObj)
            );
        });
    }
}
