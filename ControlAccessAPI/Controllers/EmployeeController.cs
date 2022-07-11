using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Newtonsoft.Json.Linq;
using Engine.BL;
using Engine.BO;
using Engine.Constants;

namespace ControlAccess.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private EmployeeBL bl {get; set;} = new EmployeeBL();

    [HttpGet]
    public List<Employee> GetEmployees() {
        return bl.GetEmployees(null);
    }    
    
    [HttpGet("{id:int}")]
    public Employee GetEmployee(int id) {
        var emps = bl.GetEmployees(id);

        if(emps != null && emps.Count > 0) {
            return emps[0];
        }
        
        return null;
    }

    [HttpPost]
    public Result SetEmployee(dynamic obj) {
        Result result = new Result();
        Employee employee = new Employee();

        if(obj != null ) {
            JObject jObj = JObject.Parse(obj.ToString());            
        }        

        return bl.SetEmployee(null, C.GLOBAL_USER);        
    }    

    [HttpPost]
    [Route("SetLevel")]
    public Result SetEmployeeAccessLevel(int employeeId, int accessLevel) {
        return bl.SetEmployeeAccessLevel(employeeId, accessLevel, C.GLOBAL_USER);
    }

    public static void PropertyFinder(IDictionary props, JObject obj){

    }    

}