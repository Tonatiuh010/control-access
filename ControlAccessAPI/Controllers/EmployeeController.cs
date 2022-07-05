using Microsoft.AspNetCore.Mvc;

namespace ControlAccess.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{          
    [HttpGet]
    public string Get() => "Just a call";


    
}