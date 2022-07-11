using Microsoft.AspNetCore.Mvc;

namespace ControlAccess.Controllers;

[Route("[controller]")]
[ApiController]
public class CheckController : ControllerBase
{          
    [HttpGet]
    public string Get() => "Just a call";

    [HttpPost]
    [Route("")]
    public string Set() => "";
       
}