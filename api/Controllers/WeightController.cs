using api.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using service.Models;
using service.Services;

namespace api.Controllers;

[ApiController]
[Route("api/v1/weight")]
public class WeightController(WeightService weightService) : Controller
{
    [RequireAuthentication]
    [HttpPost]
    public IActionResult AddWeight([FromBody] WeightInputCommandModel model)
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        return Ok(weightService.AddWeight(model, data.UserId));
    }
    
    [RequireAuthentication]
    [HttpGet]
    public IActionResult GetWeightsForUser()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        return Ok(weightService.GetAllWeightForUser(data.UserId));
    }
}