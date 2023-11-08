using Microsoft.AspNetCore.Mvc;
using service.Models;
using service.Services;

namespace api.Controllers;

[ApiController]
[Route("api/v1/account")]
public class AccountController(AccountService accountService) : Controller
{
    [HttpPost]
    [Route("register")]
    public IActionResult Register([FromBody] RegisterCommandModel model)
    {
        var user = accountService.Register(model);
        return Ok(user);
    }
}