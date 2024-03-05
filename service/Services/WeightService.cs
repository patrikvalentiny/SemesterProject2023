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
            UserId = userId,
            BodyFatPercentage = model.BodyFatPercentage,
            SkeletalMuscleWeight = model.SkeletalMuscleWeight
        };
        return weightRepository.Create(latestWeight);
    }

    public IEnumerable<WeightInput> GetAllWeightForUser(int dataUserId)
    {
        return weightRepository.GetAllWeightsForUser(dataUserId);
        
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
            UserId = userId,
            BodyFatPercentage = model.BodyFatPercentage,
            SkeletalMuscleWeight = model.SkeletalMuscleWeight
        };
        return weightRepository.Update(latestWeight);
    }

    public IEnumerable<WeightInput> AddMultipleWeights(WeightInputCommandModel[] weights, int userId)
    {
        return weights.Select(weight => AddWeight(weight, userId));
    }
}