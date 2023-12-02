using System.Linq.Expressions;
using infrastructure.DataModels;
using infrastructure.Repositories;
using Microsoft.AspNetCore.Routing;
using Serilog;

namespace service.Services;

public class StatisticsService(WeightRepository weightRepository, IRepository<UserDetails> userDetailsRepository)
{
    public IEnumerable<WeightInput> GetCurrentTrend(int dataUserId)
    {
        // get user for target date 
        var user = userDetailsRepository.GetById(dataUserId);
        if (user == null) throw new Exception("User details not found");
        
        // get all weights for user
        List<WeightInput> weights = weightRepository.GetAllWeightsForUser(dataUserId).ToList();
        if (weights.Count == 0) throw new Exception("No weights found");
        // calculate average loss per day
        var oldestWeightInput = weights.First();
        var newestWeightInput = weights.Last();
        var totalLoss = GetCurrentTotalLoss(weights); // start weight - current weight ex. 100 - 90 = 10
        var firstToLastDateDaysDiff = FirstToLastDateDaysDiff(weights); // days between start and current weight ex. -10
        var averageLoss = AverageLoss(totalLoss, firstToLastDateDaysDiff); // average loss per day ex. -1
        
        // get target date
        var targetDate = user.TargetDate ?? newestWeightInput.Date; // if target date is null, use last input date
        
        // create list of dates from first date to target date
        var dates = new List<DateTime>();
        for (var dt = oldestWeightInput.Date; dt <= targetDate; dt = dt.AddDays(1))
        {
            dates.Add(dt);
        }
        
        // for each date, calculate weight
        return dates.Select(dt => new WeightInput
        {
            Weight = decimal.Round(oldestWeightInput.Weight - ((oldestWeightInput.Date - dt.Date).Days) * averageLoss, 2), // start weight - days since start * average loss per day
            Date = dt.Date,
            UserId = dataUserId
        });
    }

    private decimal AverageLoss(decimal totalLoss, int firstToLastDateDaysDiff)
    {
        return decimal.Round(totalLoss / firstToLastDateDaysDiff, 4);
    }
    
    public decimal AverageLoss(int dataUserId)
    {
        var weights = weightRepository.GetAllWeightsForUser(dataUserId).ToList();
        var totalLoss = GetCurrentTotalLoss(weights);
        var firstToLastDateDaysDiff = FirstToLastDateDaysDiff(weights);
        return AverageLoss(totalLoss, firstToLastDateDaysDiff);
    }

    private int FirstToLastDateDaysDiff(IReadOnlyCollection<WeightInput> weights)
    {
        return (weights.First().Date - weights.Last().Date).Days;
    }
    
    public int FirstToLastDateDaysDiff(int userId)
    {
        var weights = weightRepository.GetAllWeightsForUser(userId).ToList();
        return FirstToLastDateDaysDiff(weights);
    }
    
    public decimal GetCurrentTotalLoss(int dataUserId)
    {
        var weights = weightRepository.GetAllWeightsForUser(dataUserId).ToList();
        return GetCurrentTotalLoss(weights);
    }
    
    private decimal GetCurrentTotalLoss(IReadOnlyCollection<WeightInput> weights)
    {
        if (weights.Count == 0) throw new Exception("No weights found");
        return weights.First().Weight - weights.Last().Weight;
    }

    public int DaysToTarget(int dataUserId)
    {
        return -(DateTime.Today.Date - userDetailsRepository.GetById(dataUserId)!.TargetDate!.Value.Date).Days;
    }

    public decimal WeightToGo(int dataUserId)
    {
        // get user for target date 
        var user = userDetailsRepository.GetById(dataUserId);
        if (user == null) throw new Exception("User details not found");

        var latestWeight = weightRepository.GetLatestWeightForUser(dataUserId);
        if (latestWeight == null) throw new Exception("No weights found");
        return latestWeight.Weight - user.TargetWeight;
    }

    public decimal PercentageLost(int dataUserId)
    {
        var user = userDetailsRepository.GetById(dataUserId);
        if (user == null) throw new Exception("User details not found");
        var weights = weightRepository.GetAllWeightsForUser(dataUserId).ToList();
        if (weights.Count == 0) throw new Exception("No weights found");
        var totalLoss = GetCurrentTotalLoss(weights);
        return decimal.Round(totalLoss / (weights.First().Weight - user.TargetWeight) * 100, 2);
        
    }
}