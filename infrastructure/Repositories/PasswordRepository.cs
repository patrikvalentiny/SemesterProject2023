using System.Data.Common;
using Dapper;
using infrastructure.DataModels;

namespace infrastructure.Repositories;

public class PasswordRepository(DbDataSource dataSource)
{
    public PasswordHash GetByEmail(string email)
    {
        const string sql = $@"
SELECT 
    user_id as {nameof(PasswordHash.UserId)},
    password_hash as {nameof(PasswordHash.Hash)},
    salt as {nameof(PasswordHash.Salt)},
    algorithm as {nameof(PasswordHash.Algorithm)}
FROM weight_tracker.passwords
JOIN weight_tracker.users ON passwords.user_id = users.id
WHERE email = @email;
";
        using var connection = dataSource.OpenConnection();
        return connection.QuerySingle<PasswordHash>(sql, new { email });
    }

    public void Create(int userId, string hash, string salt, string algorithm)
    {
        const string sql = $@"
INSERT INTO weight_tracker.passwords (user_id, password_hash, salt, algorithm)
VALUES (@userId, @hash, @salt, @algorithm)
";
        using var connection = dataSource.OpenConnection();
        connection.Execute(sql, new { userId, hash, salt, algorithm });
    }
    
    public PasswordHash Update(PasswordHash entity)
    {
        throw new NotImplementedException();
    }

    public PasswordHash Delete(int id)
    {
        throw new NotImplementedException();
    }
}