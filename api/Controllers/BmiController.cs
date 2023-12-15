using api.Filters;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
        try
        {
            return Ok(service.GetLatestBmi(data.UserId));
        }
        catch (Exception e)
        {
            Log.Error(e, "Error getting latest BMI");
            return BadRequest("Error getting latest BMI");
        }
    }

    [HttpGet]
    public IActionResult GetAllBmi()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(service.GetAllBmiForUser(data.UserId));
        }
        catch (Exception e)
        {
            Log.Error(e, "Error getting all BMI");
            return BadRequest("Error getting all BMI");
        }
    }
}