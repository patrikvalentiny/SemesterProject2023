using infrastructure.DataModels;
using infrastructure.Repositories;
using service.Models;

namespace service.Services;

public class WeightService(WeightRepository weightRepository)
{
    public WeightInput AddWeight(WeightInputCommandModel model, int userId)
    {
        var latestWeight = new WeightInput
        {
            Weight = model.Weight,
            Date = model.Date,
            UserId = userId
        };
        return weightRepository.Create(latestWeight);
    }

    public IEnumerable<WeightInput> GetAllWeightForUser(int dataUserId)
    {
        var weights = weightRepository.GetAllWeightsForUser(dataUserId).ToList();
        for (var i = 0; i < weights.Count; i++)
            weights[i].Difference = i == 0 ? 0 : weights[i].Weight - weights[i - 1].Weight;
        return weights;
    }

    public WeightInput DeleteWeight(DateTime date, int dataUserId)
    {
        return weightRepository.Delete(date, dataUserId);
    }

    public WeightInput UpdateWeight(WeightInputCommandModel model, int userId)
    {
        var latestWeight = new WeightInput
        {
            Weight = model.Weight,
            Date = model.Date,
            UserId = userId
        };
        return weightRepository.Update(latestWeight);
    }

    public IEnumerable<WeightInput> AddMultipleWeights(WeightInputCommandModel[] weights, int userId)
    {
        return weights.Select(weight => AddWeight(weight, userId));
    }
}