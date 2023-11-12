using api.Filters;
using Microsoft.AspNetCore.Mvc;
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
        var user = accountService.Register(model);
        return Ok(user);
    }
    
    [HttpPost]
    [Route("login")]
    public IActionResult Login([FromBody] LoginCommandModel model)
    {
        var user = accountService.Authenticate(model);
        if (user == null) return Unauthorized();
        var token = jwtService.IssueToken(SessionData.FromUser(user!));
        Response.Headers.Append("Authorization", $"Bearer {token}");
        return Ok(user);
    }
    
    [RequireAuthentication]
    [HttpGet]
    [Route("/api/account/whoami")]
    public IActionResult WhoAmI()
    {
        var data = HttpContext.GetSessionData();
        var user = accountService.Get(data!);
        return Ok(user);
    }
}