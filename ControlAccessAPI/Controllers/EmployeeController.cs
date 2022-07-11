using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Engine.BL;
using Engine.BO;
using Engine.Constants;

namespace ControlAccess.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private EmployeeBL bl {get; set;} = new EmployeeBL();

    [HttpGet]
    public List<Employee> GetEmployees() {
        return bl.GetEmployees(null);
    }    
    
    [HttpGet]
    public Employee GetEmployee(int employeeId) {
        var emps = bl.GetEmployees(employeeId);

        if(emps != null && emps.Count > 0) {
            return emps[0];
        }
        
        return null;
    }

    [HttpPost]
    public Result SetEmployee(dynamic obj) {
        return bl.SetEmployee(null, C.GLOBAL_USER);        
    }


    [HttpPost]
    [Route("SetLevel")]
    public Result SetEmployeeAccessLevel(int employeeId, int accessLevel) {
        return bl.SetEmployeeAccessLevel(employeeId, accessLevel, C.GLOBAL_USER);
    }
}