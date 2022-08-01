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
    public class CatalogController : CustomController
    {
        [HttpGet]
        [Route("Assets")]
        public Result GetEmployeeAssets() => RequestResponse(
            () => bl.GetShifts(),
            () => bl.GetPositions(),
            () => bl.GetCards(assigned:false),
            () => bl.GetAccessLevels()
        );

    }
}
