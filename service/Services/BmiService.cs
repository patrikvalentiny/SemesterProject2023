using infrastructure.DataModels;
using infrastructure.Repositories;
using service.Models;

namespace service.Services;

public class BmiService(WeightRepository weightRepository, IRepository<UserDetails> userDetailsRepository)
{
    public BmiCommandModel GetLatestBmi(int dataUserId)
    {
        var weight = weightRepository.GetLatestWeightForUser(dataUserId);
        if (weight == null) throw new Exception("No weight found");
        var userDetails = userDetailsRepository.GetById(dataUserId);
        if (userDetails == null) throw new Exception("No user details found");
        var heightInMeters = userDetails.Height / 100m;
        var bmi = weight.Weight / (heightInMeters * heightInMeters);
        return new BmiCommandModel
        {
            Bmi = decimal.Round(bmi, 2),
            Date = weight.Date,
            Category = CategorizeBmi(bmi)
        };
    }

    public IEnumerable<BmiCommandModel>? GetAllBmiForUser(int dataUserId)
    {
        var weights = weightRepository.GetAllWeightsForUser(dataUserId);
        if (weights == null) throw new Exception("No weights found");
        var userDetails = userDetailsRepository.GetById(dataUserId);
        if (userDetails == null) throw new Exception("No user details found");
        var heightInMeters = userDetails.Height / 100m;
        return weights.Select(weight => new BmiCommandModel
        {
            Bmi = decimal.Round(weight.Weight / (heightInMeters * heightInMeters), 2),
            Date = weight.Date,
            Category = CategorizeBmi(weight.Weight / (heightInMeters * heightInMeters))
        });
    }

    private string CategorizeBmi(decimal bmi)
    {
        return bmi switch
        {
            < 18.5m => "Underweight",
            < 25m => "Normal",
            < 30m => "Overweight",
            _ => "Obese"
        };
    }
}