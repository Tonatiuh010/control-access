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
public class EmployeeController : CustomContoller
{
    private ControlAccessBL bl {get; set;} = new ControlAccessBL();    

    [HttpGet]
    public Result GetEmployees() => RequestResponse(() => bl.GetEmployees());
    
    [HttpGet("{id:int?}")]
    public Result GetEmployee(int? id) => RequestResponse(() => {
        var emps = bl.GetEmployees(id);

        if(emps != null && emps.Count > 0) {
            return emps[0];
        }
        
        return null;
    });

    [HttpPost]
    public Result SetEmployee(dynamic obj) => RequestResponse(() => {
        Employee employee;
        
        JObject jObj = JObject.Parse(obj.ToString());
        employee = new Employee() {
            Id = JsonProperty<int?>.GetValue("id", jObj),
            Name = JsonProperty<string>.GetValue("name", jObj, OnMissingProperty),
            LastName = JsonProperty<string>.GetValue("lastName", jObj, OnMissingProperty),
            Job = new Position() {                    
                PositionId = JsonProperty<int?>.GetValue("position", jObj),
            },
            Shift = new Shift() {
                Id = JsonProperty<int?>.GetValue("shift", jObj),
            }
        };

        return bl.SetEmployee(employee, C.GLOBAL_USER);
    });

    [HttpPost]
    [Route("SetLevel")]
    public Result SetEmployeeAccessLevel(int employeeId, int accessLevel) => RequestResponse(() => 
        bl.SetEmployeeAccessLevel(employeeId, accessLevel, C.GLOBAL_USER)
    );
}