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
    [HttpGet]
    public Result GetEmployees() => RequestResponse(() => bl.GetEmployees());
    
    [HttpGet("{id:int?}")]
    public Result GetEmployee(int? id) => RequestResponse(() => {
        var emps = bl.GetEmployees(id);

        if(emps != null && emps.Count > 0) {
            return emps[0];
        }
        
        return "Empleado no encontrado";
    });

    [HttpPost]
    public Result SetEmployee(dynamic obj) => RequestResponse(() => {       
        JObject jObj = JObject.Parse(obj.ToString());
        Employee employee = new Employee() {
            Id = JsonProperty<int?>.GetValue("id", jObj),
            Name = JsonProperty<string>.GetValue("name", jObj, OnMissingProperty),
            LastName = JsonProperty<string>.GetValue("lastName", jObj, OnMissingProperty),
            Job = new Position() {                    
                PositionId = JsonProperty<int?>.GetValue("position", jObj),
            },
            Shift = new Shift() {
                Id = JsonProperty<int?>.GetValue("shift", jObj),
            },
            Image = new ImageData(JsonProperty<string>.GetValue("image", jObj))
        };

        var levels = JsonProperty<JArray>.GetValue("accessLevels", jObj);

        var resultInsert = bl.SetEmployee(employee, C.GLOBAL_USER);

        if (levels != null && resultInsert.Status == C.OK)
        {            
            int? employeeId = resultInsert.InsertDetails.Id;
            var employeeLevels = bl.GetEmployeeAccessLevels(employeeId);
            var incomingLevels = levels.Select(x => bl.GetAccessLevel(x.ToString())).ToList();

            foreach(var level in incomingLevels)
            {
                if(level != null && level.IsValid() && !employeeLevels.Any(x => x.Id == level.Id)) 
                {
                    bl.SetEmployeeAccessLevel((int)employeeId, (int)level.Id, true, C.GLOBAL_USER);
                }
            }

            foreach(var empLevel in employeeLevels)
            {
                if (!incomingLevels.Any(x => x != null && x.IsValid() && x.Id == empLevel.Id ))
                {
                    bl.SetEmployeeAccessLevel((int)employeeId, (int)empLevel.Id, false, C.GLOBAL_USER);
                }
            }

        }

        return bl.SetEmployee(employee, C.GLOBAL_USER);
    });

    [HttpPost]
    [Route("DownEmployee")]
    public Result SetDownEmployee(dynamic obj) => RequestResponse(
        () => (Result)bl.SetDownEmployee(
            JsonProperty<int>.GetValue(
                "id", 
                JObject.Parse(obj.ToString()), 
                OnMissingProperty
            ),
            C.GLOBAL_USER
        )
    );

    [HttpPost]
    [Route("SetLevel")]
    public Result SetEmployeeAccessLevel(dynamic obj) => RequestResponse(() =>
    {
        JObject jObj = JObject.Parse(obj.ToString());
        return bl.SetEmployeeAccessLevel(
            JsonProperty<int>.GetValue("employee", jObj, OnMissingProperty),
            JsonProperty<int>.GetValue("level", jObj, OnMissingProperty),
            JsonProperty<bool>.GetValue("status", jObj, OnMissingProperty),
            C.GLOBAL_USER
        );
    });
}