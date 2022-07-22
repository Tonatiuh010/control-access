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
    private readonly ControlAccessBL bl = new ControlAccessBL();

    [HttpGet]
    public Result GetCards() => RequestResponse(() => "This is my basic response.");

    [HttpGet("{id:int?}")]
    public Result GetCard(int id) => RequestResponse(() => "This is my basic return call");

    [HttpPost]
    public Result SetCard(dynamic obj) => RequestResponse(() => {
        JObject jObj = new JObject(obj.ToString());
        Card card = new Card(){
            Id = JsonProperty<int?>.GetValue("id", jObj),
            Employee = new Employee() {
                Id = JsonProperty<int?>.GetValue("employeeId", jObj, OnMissingProperty)
            },
            Key = JsonProperty<string>.GetValue("serial", jObj, OnMissingProperty)
        };

        return bl.SetCard(card, C.GLOBAL_USER);
    });

    [HttpPost]
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