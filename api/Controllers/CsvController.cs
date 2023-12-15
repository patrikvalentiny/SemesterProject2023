using api.Filters;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using service.Services;

namespace api.Controllers;

[RequireAuthentication]
[Route("api/v1/csv")]
[ApiController]
public class CsvController(CsvService csvService) : Controller
{
    [HttpPost]
    public IActionResult UploadCsv([FromForm] IFormFile file)
    {
        Log.Information(file.FileName);
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return Ok(csvService.ParseCsv(data.UserId, file));
        }
        catch (Exception e)
        {
            Log.Error(e, "Error parsing CSV file");
            return BadRequest("Error parsing CSV file");
        }
    }

    [HttpGet]
    public IActionResult DownloadCsv()
    {
        var data = HttpContext.GetSessionData();
        if (data == null) return Unauthorized();
        try
        {
            return File(csvService.CreateCsv(data.UserId), "text/csv", "weights.csv");
        }
        catch (Exception e)
        {
            Log.Error(e, "Error creating CSV file");
            return BadRequest("Error creating CSV file");
        }
    }
}