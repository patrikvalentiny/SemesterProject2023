using infrastructure.DataModels;
using infrastructure.Repositories;
using service.Models;

namespace service.Services;

public class BmiService(WeightRepository weightRepository, IRepository<UserDetails> userDetailsRepository)
{
    public BmiCommandModel? GetLatestBmi(int dataUserId)
    {
        var weight = weightRepository.GetLatestWeightForUser(dataUserId);
        var userDetails = userDetailsRepository.GetById(dataUserId);
        if (weight == null || userDetails == null) return null;
        var heightInMeters = userDetails.Height / 100m;
        return new BmiCommandModel
        {
            Bmi = decimal.Round(weight.Weight / (heightInMeters * heightInMeters), 2),
            Date = weight.Date
        };
    }
    
    public IEnumerable<BmiCommandModel>? GetAllBmiForUser(int dataUserId)
    {
        var weights = weightRepository.GetAllWeightsForUser(dataUserId);
        var userDetails = userDetailsRepository.GetById(dataUserId);
        if (userDetails == null) return null;
        var heightInMeters = userDetails.Height / 100m;
        return weights.Select(weight => new BmiCommandModel
        {
            Bmi = decimal.Round(weight.Weight / (heightInMeters * heightInMeters), 2),
            Date = weight.Date
        });
    }
}