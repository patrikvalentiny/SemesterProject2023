using System.Globalization;
using CsvHelper;
using infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace service.Services;

public class CsvService(WeightRepository weightRepository)
{

    public object? ParseCsv(int dataUserId, IFormFile formFile)
    {
        Log.Information("Parsing CSV file");
        Log.Information(dataUserId.ToString());
        Log.Information(formFile.FileName);
        return null;
    }
    
    public byte[] CreateCsv(int dataUserId)
    {
        var weights = weightRepository.GetAllWeightsForUser(dataUserId).ToList();
        var weightsStripped = weights.Select(w => new {w.Weight, w.Date.Date});
        using var resource = new MemoryStream();
        using var writer = new StreamWriter(resource);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(weightsStripped);
        csv.Flush();
        resource.Position = 0;
        return resource.ToArray();
    }
}