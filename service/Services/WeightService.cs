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
}