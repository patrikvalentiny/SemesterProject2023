using infrastructure.DataModels;
using infrastructure.Repositories;

namespace service.Services;

public class UserDetailsService(IRepository<UserDetails> userDetailsRepository)
{
    public UserDetails? GetUserDetails(int dataUserId)
    {
        return userDetailsRepository.GetById(dataUserId);
    }

    public UserDetails AddUserDetails(UserDetails model)
    {
        return userDetailsRepository.Create(model);
    }

    public UserDetails UpdateUserDetails(UserDetails model)
    {
        return userDetailsRepository.Update(model);
    }
}