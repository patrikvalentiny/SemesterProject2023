using api.Filters;
using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace api.Controllers;

[RequireAuthentication]
[ApiController]
[Route("api/v1/bmi")]
public class BmiController(BmiService service) : Controller
{
    [HttpGet("latest")]
    public IActionResult GetBmi()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        return Ok(service.GetLatestBmi(data.UserId));
    }
    
    [HttpGet]
    public IActionResult GetAllBmi()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        return Ok(service.GetAllBmiForUser(data.UserId));
    }
}