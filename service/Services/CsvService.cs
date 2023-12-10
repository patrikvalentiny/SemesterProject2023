using Microsoft.AspNetCore.Http;
using Serilog;

namespace service.Services;

public class CsvService
{

    public object? ParseCsv(int dataUserId, IFormFile formFile)
    {
        Log.Information("Parsing CSV file");
        Log.Information(dataUserId.ToString());
        Log.Information(formFile.FileName);
        return null;
    }
}