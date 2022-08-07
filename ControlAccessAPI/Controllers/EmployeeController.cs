using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Newtonsoft.Json.Linq;
using Engine.BL;
using Engine.BL.Delegates;
using Engine.BO;
using Classes;
using Engine.Constants;
using static System.Net.Mime.MediaTypeNames;

namespace ControlAccess.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : CustomController
{
    [HttpGet]
    public Result GetEmployees() => RequestResponse(() => bl.GetEmployees());
    
    [HttpGet("{id:int?}")]
    public Result GetEmployee(int? id) => RequestResponse(() =>
        GetItem(bl.GetEmployees(id))
    );

    [HttpPost]  
    public Result SetEmployee(dynamic obj) => RequestResponse(() => {       
        JObject jObj = JObject.Parse(obj.ToString());

        ImageData? img = null;

        var imgString = JsonProperty<string>.GetValue("image", jObj);

        if (!string.IsNullOrEmpty(imgString))
        {
            if (Uri.TryCreate(imgString, UriKind.Absolute, out Uri uri))
            {
                img = new ImageData(ImageData.GetBytesFromUrl(uri.AbsoluteUri));
            }
            else
            {
                img = imgString.Contains("base64") ? new ImageData(imgString) : new ImageData();
            }
        }

        Engine.BO.ControlAccess employee = new Engine.BO.ControlAccess() {
            Id = JsonProperty<int?>.GetValue("id", jObj),
            Name = JsonProperty<string>.GetValue("name", jObj, OnMissingProperty),
            LastName = JsonProperty<string>.GetValue("lastName", jObj, OnMissingProperty),
            Job = new Position() {                    
                PositionId = JsonProperty<int?>.GetValue("position", jObj),
            },
            Shift = new Shift() {
                Id = JsonProperty<int?>.GetValue("shift", jObj),
            },
            Image = img,
            Card = new Card()
            {
                Id = JsonProperty<int?>.GetValue("card", jObj)
            }
        };

        var levels = JsonProperty<JArray>.GetValue("accessLevels", jObj);

        var resultInsert = bl.SetEmployee(employee, C.GLOBAL_USER);

        if ( resultInsert.Status == C.OK )
        {
            int? employeeId = resultInsert.InsertDetails.Id;

            if (employee.Card != null && employee.Card.IsValid())
            {

                var employeeCard = bl.GetCards(assigned: true).Find(x => x.Employee == employeeId);

                if (employeeCard != null && employeeCard.IsValid())
                    bl.SetDownCard((int)employeeCard.Id, C.GLOBAL_USER);

                var chResult = bl.SetCard(new CardEmployee(employee.Card, employeeId), C.GLOBAL_USER);
                
            }

            if (levels != null)
            {                
                var employeeLevels = bl.GetEmployeeAccessLevels(employeeId);
                var incomingLevels = levels.Select(x => bl.GetAccessLevel(x.ToString())).ToList();

                foreach (var level in incomingLevels)
                {
                    if (level != null && level.IsValid() && !employeeLevels.Any(x => x.Id == level.Id))
                    {
                        bl.SetEmployeeAccessLevel((int)employeeId, (int)level.Id, true, C.GLOBAL_USER);
                    }
                }

                foreach (var empLevel in employeeLevels)
                {
                    if (!incomingLevels.Any(x => x != null && x.IsValid() && x.Id == empLevel.Id))
                    {
                        bl.SetEmployeeAccessLevel((int)employeeId, (int)empLevel.Id, false, C.GLOBAL_USER);
                    }
                }

            }
        }        

        return resultInsert;
    });

    [HttpGet("image/{id:int?}")]    
    public IActionResult GetEmployeeImage(int? id)
    {
        byte[] bytes = Array.Empty<byte>();
        var obj = GetItem(bl.GetEmployees(id));

        if(obj != null)
        {
            var emp = (Engine.BO.ControlAccess)obj;
            var imgBytes = emp.Image?.Bytes;

            if(imgBytes != null)
            {
                bytes = imgBytes;
            }
        }

        return File(bytes, "image/jpeg");
    }

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