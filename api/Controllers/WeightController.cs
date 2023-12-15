using api.Dtos;
using api.Filters;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using service.Models;
using service.Services;

namespace api.Controllers;

[RequireAuthentication]
[ApiController]
[Route("api/v1/weight")]
public class WeightController(WeightService weightService) : Controller
{
    [HttpPost]
    public IActionResult AddWeight([FromBody] WeightInputCommandModel model)
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            var weight = weightService.AddWeight(model, data.UserId);
            var weightDto = new WeightDto
            {
                Weight = weight.Weight,
                Date = weight.Date,
                Difference = weight.Difference
            };
            return Ok(weightDto);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error adding weight");
            return BadRequest("Error adding weight");
        }
    }

    [HttpPut]
    public IActionResult UpdateWeight([FromBody] WeightInputCommandModel model)
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            var weight = weightService.UpdateWeight(model, data.UserId);
            var weightDto = new WeightDto
            {
                Weight = weight.Weight,
                Date = weight.Date,
                Difference = weight.Difference
            };
            return Ok(weightDto);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error updating weight");
            return BadRequest("Error updating weight");
        }
    }


    [HttpGet]
    public IActionResult GetWeightsForUser()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            var weights = weightService.GetAllWeightForUser(data.UserId);
            var weightDtos = weights.Select(weight => new WeightDto
            {
                Weight = weight.Weight,
                Date = weight.Date,
                Difference = weight.Difference
            });
            return Ok(weightDtos);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error getting weights");
            return BadRequest("Error getting weights");
        }
    }


    [HttpDelete("{date:datetime}")]
    public IActionResult DeleteWeight([FromRoute] DateTime date)
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            var weight = weightService.DeleteWeight(date, data.UserId);
            var weightDto = new WeightDto
            {
                Weight = weight.Weight,
                Date = weight.Date,
                Difference = weight.Difference
            };
            return Ok(weightDto);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error deleting weight");
            return BadRequest("Error deleting weight");
        }
    }

    [HttpPost("multiple")]
    public IActionResult AddMultipleWeights([FromBody] WeightInputCommandModel[] weights)
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(weightService.AddMultipleWeights(weights, data.UserId).Select(weight => new WeightDto
            {
                Weight = weight.Weight,
                Date = weight.Date,
                Difference = weight.Difference
            }));
        }
        catch (Exception e)
        {
            Log.Error(e, "Error adding multiple weights");
            return BadRequest("Error adding multiple weights");
        }
    }
}