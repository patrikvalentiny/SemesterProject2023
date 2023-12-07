﻿using api.Filters;
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
        return Ok(weightService.AddWeight(model, data.UserId));
    }

    [HttpPut]
    public IActionResult UpdateWeight([FromBody] WeightInputCommandModel model)
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        var input = new WeightInput
        {
            Weight = model.Weight,
            Date = model.Date,
            UserId = data.UserId
        };
        return Ok(weightService.UpdateWeight(input));
    }


    [HttpGet]
    public IActionResult GetWeightsForUser()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        return Ok(weightService.GetAllWeightForUser(data.UserId));
    }


    [HttpDelete("{date:datetime}")]
    public IActionResult DeleteWeight([FromRoute] DateTime date)
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        return Ok(weightService.DeleteWeight(date, data.UserId));
    }
}