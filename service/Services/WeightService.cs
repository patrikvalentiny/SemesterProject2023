using infrastructure.DataModels;
using infrastructure.Repositories;
using service.Models;

namespace service.Services;

public class WeightService(WeightRepository weightRepository)
{
    public  WeightInput AddWeight(WeightInputCommandModel model, int userId)
    {
        WeightInput? latestWeight = new WeightInput()
        {
            Weight = model.Weight,
            Date = model.Date,
            UserId = userId
        };
        return weightRepository.Create(latestWeight);
    }
    
    public WeightInput? GetLatestWeightForUser(int userId)
    {
        return weightRepository.GetLatestWeightForUser(userId);
    }
    public IEnumerable<WeightInput> GetAllWeightForUser(int dataUserId)
    {
        return weightRepository.GetAllWeightsForUser(dataUserId);
    }

    public WeightInput DeleteWeight(DateTime date, int dataUserId)
    {
        return weightRepository.Delete(date, dataUserId);
    }

    public WeightInput UpdateWeight(WeightInput model)
    {
        return weightRepository.Update(model);
    }
}