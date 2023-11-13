using api.Filters;
using infrastructure.DataModels;
using Microsoft.AspNetCore.Http.HttpResults;
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
            {
                
                return BadRequest("Username already exists");
            }
            else if (e.Message.Contains("email_uk"))
            {
                return BadRequest("Email already exists");
            } 
            else
            {
                return BadRequest("Unknown error");
            }
            
        }
        var token = jwtService.IssueToken(SessionData.FromUser(user));
        Response.Headers.Append("Authorization", $"Bearer {token}");
        return Ok(new {user, token});
    }
    
    [HttpPost]
    [Route("login")]
    public IActionResult Login([FromBody] LoginCommandModel model)
    {
        var user = accountService.Authenticate(model);
        if (user == null) return Unauthorized();
        
        var token = jwtService.IssueToken(SessionData.FromUser(user));
        Response.Headers.Append("Authorization", $"Bearer {token}");
        return Ok(new {user, token});
    }
    
    [RequireAuthentication]
    [HttpGet]
    [Route("whoami")]
    public IActionResult WhoAmI()
    {
        var data = HttpContext.GetSessionData();
        var user = accountService.Get(data!);
        return Ok(user);
    }
}