using api.Dtos;
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
    
    [HttpGet("averageLoss")]
    public IActionResult GetAverageLoss()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(-service.AverageLoss(data.UserId));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [HttpGet("averageLossPerWeek")]
    public IActionResult GetAverageLossPerWeek()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(-service.AverageLoss(data.UserId) * 7);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [HttpGet("daysIn")]
    public IActionResult GetDaysIn()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(-service.FirstToLastDateDaysDiff(data.UserId));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [HttpGet("daysToTarget")]
    public IActionResult GetDaysToGo()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(service.DaysToTarget(data.UserId));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [HttpGet("weightToGo")]
    public IActionResult GetWeightToGo()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(service.WeightToGo(data.UserId));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [HttpGet("percentageOfGoal")]
    public IActionResult GetPercentageLost()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(service.PercentageLost(data.UserId));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [HttpGet("predictedTargetDate")]
    public IActionResult GetPredictedTargetDate()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(service.PredictedTargetDate(data.UserId));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [HttpGet("predictedTargetWeight")]
    public IActionResult GetPredictedTargetWeight()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(service.GetPredictedWeightOnTargetDate(data.UserId));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [HttpGet("bmiChange")]
    public IActionResult GetBmiChange()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(service.BmiChange(data.UserId));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [HttpGet("lowestWeight")]
    public IActionResult GetLowestWeight()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            var weightDto = new WeightDto
            {
                Weight = service.GetLowestWeight(data.UserId).Weight,
                Date = service.GetLowestWeight(data.UserId).Date,
                Difference = service.GetLowestWeight(data.UserId).Difference
            };
            return Ok(service.GetLowestWeight(data.UserId));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
}