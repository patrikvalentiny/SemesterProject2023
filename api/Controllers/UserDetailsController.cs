using api.Filters;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using service.Models;
using service.Services;

namespace api.Controllers;

[RequireAuthentication]
[ApiController]
[Route("api/v1/profile")]
public class UserDetailsController(UserDetailsService userDetailsService) : Controller
{
    [HttpGet]
    public IActionResult GetUserDetails()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(userDetailsService.GetUserDetails(data.UserId));
        }
        catch (Exception e)
        {
            Log.Error(e, "Error getting user details");
            return BadRequest("Error getting user details");
        }
    }


    [HttpPost]
    public IActionResult AddUserDetails([FromBody] UserDetailsCommandModel model)
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(userDetailsService.AddUserDetails(model, data.UserId));
        }
        catch (Exception e)
        {
            Log.Error(e, "Error adding user details");
            return BadRequest("Error adding user details");
        }
    }

    [HttpPut]
    public IActionResult UpdateUserDetails([FromBody] UserDetailsCommandModel model)
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(userDetailsService.UpdateUserDetails(model, data.UserId));
        }
        catch (Exception e)
        {
            Log.Error(e, "Error updating user details");
            return BadRequest("Error updating user details");
        }
    }
}