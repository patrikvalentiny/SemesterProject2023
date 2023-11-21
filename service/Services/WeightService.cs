using infrastructure.DataModels;
using infrastructure.Repositories;
using service.Models;

namespace service.Services;

public class WeightService(WeightRepository weightRepository)
{
    public  WeightInput AddWeight(WeightInputCommandModel model, int userId)
    {
        return weightRepository.Create(model.Weight, model.Date, userId);
    }
    
    public WeightInput? GetLatestWeightForUser(int userId)
    {
        return weightRepository.GetLatestWeightForUser(userId);
    }
    public IEnumerable<WeightInput> GetAllWeightForUser(int dataUserId)
    {
        return weightRepository.GetAllWeightsForUser(dataUserId);
    }

    public WeightInput DeleteWeight(int id, int dataUserId)
    {
        return weightRepository.Delete(id, dataUserId);
    }
}