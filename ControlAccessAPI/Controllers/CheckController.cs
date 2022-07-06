using Microsoft.AspNetCore.Mvc;

namespace ControlAccess.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckController : ControllerBase
{          
    [HttpGet]
    public string Get() => "Just a call";

    [HttpPost]
    [Route("")]
    public string Set() => "";
       
}