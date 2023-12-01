using api.Filters;
using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace api.Controllers;

[RequireAuthentication]
[ApiController]
[Route("api/v1/statistics")]
public class StatisticsController(StatisticsService service) : Controller
{
    
    [HttpGet("currentTrend")]
    public IActionResult GetCurrentTrend()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(service.GetCurrentTrend(data.UserId));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [HttpGet("currentTotalLoss")]
    public IActionResult GetCurrentProgress()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(service.GetCurrentTotalLoss(data.UserId));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
}