using Microsoft.AspNetCore.Mvc;
using Engine.BO;
using Engine.BL;
using Engine.Constants;

namespace ControlAccess.Controllers;

[Route("[controller]")]
[ApiController]
public class LevelController : ControllerBase
{
    private EmployeeBL bl {get; set;} = new EmployeeBL();

    [HttpGet]
    public List<AccessLevel> GetAccessLevels() => bl.GetAccessLevels();

    [HttpGet]
    public List<EmployeeAccessLevel> GetEmployeeAccessLevel(int id) => bl.GetEmployeeAccessLevels(id);

       
}