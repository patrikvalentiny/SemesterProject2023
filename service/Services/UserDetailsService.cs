using infrastructure.DataModels;
using infrastructure.Repositories;
using service.Models;

namespace service.Services;

public class UserDetailsService(IRepository<UserDetails> userDetailsRepository)
{
    public UserDetails GetUserDetails(int dataUserId)
    {
        var userDetails = userDetailsRepository.GetById(dataUserId);
        if (userDetails == null) throw new Exception("No user details found");
        return userDetails;
    }

    public UserDetails AddUserDetails(UserDetailsCommandModel model, int userId)
    {
        var user = new UserDetails
        {
            UserId = userId,
            Firstname = model.Firstname,
            Lastname = model.Lastname,
            Height = model.Height,
            TargetWeight = model.TargetWeight,
            TargetDate = model.TargetDate,
            LossPerWeek = model.LossPerWeek
        };
        return userDetailsRepository.Create(user);
    }

    public UserDetails UpdateUserDetails(UserDetailsCommandModel model, int userId)
    {
        var user = new UserDetails
        {
            UserId = userId,
            Firstname = model.Firstname,
            Lastname = model.Lastname,
            Height = model.Height,
            TargetWeight = model.TargetWeight,
            TargetDate = model.TargetDate,
            LossPerWeek = model.LossPerWeek
        };
        return userDetailsRepository.Update(user);
    }
}