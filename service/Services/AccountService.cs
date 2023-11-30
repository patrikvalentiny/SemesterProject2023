using infrastructure.DataModels;
using infrastructure.Repositories;
using Serilog;
using service.Models;
using service.Password;

namespace service.Services;

public class AccountService(IRepository<User> userRepository,
    PasswordRepository passwordRepository)
{
    public User? Authenticate(LoginCommandModel model)
    {
        try
        {
            var passwordHash = passwordRepository.GetByUsername(model.Username) ??
                               passwordRepository.GetByEmail(model.Username);
            if (passwordHash == null) return null;
            var hashAlgorithm = PasswordHashAlgorithm.Create(passwordHash.Algorithm);
            var isValid = hashAlgorithm.VerifyHashedPassword(model.Password, passwordHash.Hash, passwordHash.Salt);
            if (isValid) return userRepository.GetById(passwordHash.UserId);
        }
        catch (Exception e)
        {
            Log.Error("Authenticate error: {Message}", e);
        }

        return null;
    }

    public User Register(RegisterCommandModel model)
    {
        var hashAlgorithm = PasswordHashAlgorithm.Create();
        var salt = hashAlgorithm.GenerateSalt();
        var hash = hashAlgorithm.HashPassword(model.Password, salt);
        var user = userRepository.Create(new User
        {
            Username = model.Username,
            Email = model.Email
        });
        passwordRepository.Create(user.Id, hash, salt, hashAlgorithm.GetName());
        return user;
    }

    public User? Get(SessionData data)
    {
        return userRepository.GetById(data.UserId);
    }
}