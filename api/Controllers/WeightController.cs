using api.Dtos;
using api.Filters;
using infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
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
        var weight = weightService.AddWeight(model, data.UserId);
        var weightDto = new WeightDto
        {
            Weight = weight.Weight,
            Date = weight.Date,
            Difference = weight.Difference
        };
        return Ok(weightDto);
    }

    [HttpPut]
    public IActionResult UpdateWeight([FromBody] WeightInputCommandModel model)
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        var weight = weightService.UpdateWeight(model, data.UserId);
        var weightDto = new WeightDto
        {
            Weight = weight.Weight,
            Date = weight.Date,
            Difference = weight.Difference
        };
        return Ok(weightDto);
    }


    [HttpGet]
    public IActionResult GetWeightsForUser()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        var weights = weightService.GetAllWeightForUser(data.UserId);
        var weightDtos = weights.Select(weight => new WeightDto
        {
            Weight = weight.Weight,
            Date = weight.Date,
            Difference = weight.Difference
        });
        return Ok(weightDtos);
    }


    [HttpDelete("{date:datetime}")]
    public IActionResult DeleteWeight([FromRoute] DateTime date)
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        var weight = weightService.DeleteWeight(date, data.UserId);
        var weightDto = new WeightDto
        {
            Weight = weight.Weight,
            Date = weight.Date,
            Difference = weight.Difference
        };
        return Ok(weightDto);
    }
}