using api.Dtos;
using api.Filters;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
            Log.Error(e, "Error getting current trend");
            return BadRequest("Error getting current trend");
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
            Log.Error(e, "Error getting current total loss");
            return BadRequest("Error getting current total loss");
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
            Log.Error(e, "Error getting average loss");
            return BadRequest("Error getting average loss");
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
            Log.Error(e, "Error getting average loss per week");
            return BadRequest("Error getting average loss per week");
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
            Log.Error(e, "Error getting days in");
            return BadRequest("Error getting days in");
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
            Log.Error(e, "Error getting days to target");
            return BadRequest("Error getting days to target");
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
            Log.Error(e, "Error getting weight to go");
            return BadRequest("Error getting weight to go");
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
            Log.Error(e, "Error getting percentage lost");
            return BadRequest("Error getting percentage lost");
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
            Log.Error(e, "Error getting predicted target date");
            return BadRequest("Error getting predicted target date");
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
            Log.Error(e, "Error getting predicted target weight");
            return BadRequest("Error getting predicted target weight");
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
            Log.Error(e, "Error getting BMI change");
            return BadRequest("Error getting BMI change");
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
            Log.Error(e, "Error getting lowest weight");
            return BadRequest("Error getting lowest weight");
        }
        
    }
    
}