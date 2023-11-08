using System.Data.Common;
using Dapper;
using infrastructure.DataModels;
using Npgsql;

namespace infrastructure.Repositories;

public class PasswordRepository(DbDataSource dataSource)
{
    public PasswordHash Update(PasswordHash entity)
    {
        throw new NotImplementedException();
    }

    public PasswordHash Delete(int id)
    {
        throw new NotImplementedException();
    }

    public PasswordHash GetByEmail(string modelEmail)
    {
        throw new NotImplementedException();
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
}