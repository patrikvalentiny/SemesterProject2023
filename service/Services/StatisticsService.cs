using infrastructure.DataModels;
using infrastructure.Repositories;
using Serilog;

namespace service.Services;

public class StatisticsService(WeightRepository weightRepository, IRepository<UserDetails> userDetailsRepository)
{
    public IEnumerable<WeightInput> GetCurrentTrend(int dataUserId)
    {
        // get user for target date 
        var user = userDetailsRepository.GetById(dataUserId);
        if (user == null) throw new Exception("User not found");
        
        // get all weights for user
        List<WeightInput> weights = weightRepository.GetAllWeightsForUser(dataUserId).ToList();
        if (weights.Count == 0) throw new Exception("No weights found");
        // reverse list so that the first date is oldest date
        weights.Reverse();
        // calculate average loss per day
        var oldestWeightInput = weights.First();
        var newestWeightInput = weights.Last();
        var totalLoss = oldestWeightInput.Weight - newestWeightInput.Weight; // start weight - current weight ex. 100 - 90 = 10
        var firstToLastDateDaysDiff = (oldestWeightInput.Date - newestWeightInput.Date).Days; // days between start and current weight ex. -10
        var averageLoss = decimal.Round(totalLoss / firstToLastDateDaysDiff, 4); // average loss per day ex. -1
        
        // get target date
        var targetDate = user.TargetDate ?? oldestWeightInput.Date; // if target date is null, use last input date
        
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
}