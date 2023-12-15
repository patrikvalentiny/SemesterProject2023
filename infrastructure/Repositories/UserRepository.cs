using System.Data.Common;
using Dapper;
using infrastructure.DataModels;

namespace infrastructure.Repositories;

public class UserRepository(DbDataSource dataSource) : IRepository<User>
{
    public User Create(User user)
    {
        const string sql = $"""

                            INSERT INTO weight_tracker.users (username, email)
                            VALUES (@username, @email)
                            RETURNING
                                id as {nameof(User.Id)},
                                username as {nameof(User.Username)},
                                email as {nameof(User.Email)}
                            ;

                            """;
        using var connection = dataSource.OpenConnection();
        return connection.QueryFirst<User>(sql, new { username = user.Username, email = user.Email });
    }

    public User? GetById(int id)
    {
        const string sql = $@"
SELECT
    id as {nameof(User.Id)},
    username as {nameof(User.Username)},
    email as {nameof(User.Email)}
FROM weight_tracker.users
WHERE id = @id;
";
        using var connection = dataSource.OpenConnection();
        return connection.QueryFirstOrDefault<User>(sql, new { id });
    }

    public IEnumerable<User> GetAll()
    {
        const string sql = $@"
SELECT
    id as {nameof(User.Id)},
    username as {nameof(User.Username)},
    email as {nameof(User.Email)}
FROM users
";
        using var connection = dataSource.OpenConnection();
        return connection.Query<User>(sql);
    }

    public User Update(User user)
    {
        const string sql = $@"
UPDATE weight_tracker.users
SET
    username = @username,
    email = @email,
    firstname = @firstname,
    lastname = @lastname
WHERE id = @userId
RETURNING
    id AS {nameof(User.Id)},
    username AS {nameof(User.Username)},
    email AS {nameof(User.Email)}
;";

        using var connection = dataSource.OpenConnection();
        return connection.QueryFirst<User>(sql, new { userId = user.Id, username = user.Username, email = user.Email });
    }

    public User Delete(int id)
    {
        const string sql = $@"
DELETE FROM weight_tracker.users
WHERE id = @id
RETURNING
    id AS {nameof(User.Id)},
    username AS {nameof(User.Username)},
    email AS {nameof(User.Email)}
;";

        using var connection = dataSource.OpenConnection();
        return connection.QueryFirst<User>(sql, new { id });
    }
}