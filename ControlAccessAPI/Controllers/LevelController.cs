using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Newtonsoft.Json.Linq;
using Engine.BL;
using Engine.BL.Delegates;
using Engine.BO;
using Classes;
using Engine.Constants;

namespace ControlAccess.Controllers;

[Route("[controller]")]
[ApiController]
public class LevelController : CustomContoller
{
    private ControlAccessBL bl {get; set;} = new ControlAccessBL();

    [HttpGet]
    public Result GetAccessLevels() => RequestResponse(() => bl.GetAccessLevels());

    [HttpGet]
    public Result GetEmployeeAccessLevel(int id) => RequestResponse(() => bl.GetEmployeeAccessLevels(id));



       
}