using System;
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
public class CardController : CustomContoller 
{
    [HttpGet]
    public Result GetCards() => RequestResponse(() => "This is my basic response.");

    [HttpGet("{id:int?}")]
    public Result GetCard(int id) => RequestResponse(() => "This is my basic return call");

    [HttpPost]
    public Result SetCard(dynamic obj) => RequestResponse(() => {
        JObject jObj = new JObject(obj.ToString());
        CardEmployee card = new CardEmployee(){
            Id = JsonProperty<int?>.GetValue("id", jObj),
            Employee = new Employee() {
                Id = JsonProperty<int?>.GetValue("employeeId", jObj, OnMissingProperty)
            },
            Key = JsonProperty<string>.GetValue("serial", jObj, OnMissingProperty)
        };

        return bl.SetCard(card, C.GLOBAL_USER);
    });

    [HttpPost]
    [Route("DownCard")]
    public Result SetDownCard(dynamic obj) => RequestResponse(() => 
        bl.SetDownCard(
            JsonProperty<int>.GetValue(
                "id", 
                JObject.Parse(obj.ToString()), 
                OnMissingProperty
            ), 
            C.GLOBAL_USER
        )
    );

}