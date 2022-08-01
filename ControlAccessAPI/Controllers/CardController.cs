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
public class CardController : CustomController
{
    [HttpGet]
    public Result GetCards() => RequestResponse(() => bl.GetCards());

    [HttpGet("{id:int?}")]
    public Result GetCard(int? id) => RequestResponse(() => 
        GetItem(bl.GetCards(id)) 
    );

    [HttpPost]
    public Result SetCard(dynamic obj) => RequestResponse(() => {
        JObject jObj = JObject.Parse(obj.ToString());
        CardEmployee card = new (JsonProperty<int?>.GetValue("employee", jObj))
        {
            Id = JsonProperty<int?>.GetValue("id", jObj),
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