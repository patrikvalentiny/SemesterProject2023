using api.Filters;
using infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using service;
using service.Models;
using service.Services;

namespace api.Controllers;

[ApiController]
[Route("api/v1/account")]
public class AccountController(AccountService accountService, JwtService jwtService) : Controller
{
    [HttpPost]
    [Route("register")]
    public IActionResult Register([FromBody] RegisterCommandModel model)
    {
        User user;
        try
        {
            user = accountService.Register(model);
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            if (e.Message.Contains("username_uk"))
                return BadRequest("Username already exists");
            if (e.Message.Contains("email_uk"))
                return BadRequest("Email already exists");
            return BadRequest("Unknown error");
        }

        try
        {
            var token = jwtService.IssueToken(SessionData.FromUser(user));
            Response.Headers.Append("Authorization", $"Bearer {token}");
            return Ok(new { user, token }); 
        } catch (Exception e)
        {
            Log.Error(e, "Error issuing token");
            return BadRequest("Error issuing token");
        }
        
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login([FromBody] LoginCommandModel model)
    {
        var user = accountService.Authenticate(model);
        if (user == null) return BadRequest("Invalid username or password");

        try
        {
            var token = jwtService.IssueToken(SessionData.FromUser(user));
            Response.Headers.Append("Authorization", $"Bearer {token}");
            return Ok(new { user, token }); 
        } catch (Exception e)
        {
            Log.Error(e, "Error issuing token");
            return BadRequest("Error issuing token");
        }
        
    }
}