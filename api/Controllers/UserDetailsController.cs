using api.Filters;
using infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
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
        return Ok(userDetailsService.GetUserDetails(data.UserId));
    }


    [HttpPost]
    public IActionResult AddUserDetails([FromBody] UserDetails model)
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        model.UserId = data.UserId;
        return Ok(userDetailsService.AddUserDetails(model));
    }

    [HttpPut]
    public IActionResult UpdateUserDetails([FromBody] UserDetails model)
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        model.UserId = data.UserId;
        return Ok(userDetailsService.UpdateUserDetails(model));
    }
}