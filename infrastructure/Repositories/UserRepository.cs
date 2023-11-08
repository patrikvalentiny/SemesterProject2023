using Dapper;
using infrastructure.DataModels;
using Npgsql;

namespace infrastructure.repositories;

public class UserRepository(NpgsqlDataSource _dataSource) : IRepository<User>
{
    
    public User Create(User user)
    {
        const string sql = $@"
INSERT INTO weight_tracker.users (username, email, fistname, lastname)
VALUES (@username, @email, @firstname, @lastname)
RETURNING
    id as {nameof(User.Id)},
    username as {nameof(User.Username)},
    email as {nameof(User.Email)},
    firstname as {nameof(User.Firstname)},
    lastname as {nameof(User.Lastname)}
    ;
";
        using var connection = _dataSource.OpenConnection();
        return connection.QueryFirst<User>(sql, new { uusername = user.Username, email = user.Email, firstname= user.Firstname, lastname = user.Lastname });
    }
    
    public User GetById(int id)
    {
        const string sql = $@"
SELECT
    id as {nameof(User.Id)},
    username as {nameof(User.Username)},
    email as {nameof(User.Email)},
    firstname as {nameof(User.Firstname)},
    lastname as {nameof(User.Lastname)}
FROM weight_tracker.users
WHERE id = @id;
";
        using var connection = _dataSource.OpenConnection();
        return connection.QueryFirstOrDefault<User>(sql, new { id });
    }
    
    public IEnumerable<User> GetAll()
    {
        const string sql = $@"
SELECT
    id as {nameof(User.Id)},
    username as {nameof(User.Username)},
    email as {nameof(User.Email)},
    firstname as {nameof(User.Firstname)},
    lastname as {nameof(User.Lastname)}
FROM users
";
        using var connection = _dataSource.OpenConnection();
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
    email AS {nameof(User.Email)},
    firstname AS {nameof(User.Firstname)},
    lastname AS {nameof(User.Lastname)}
;";

        using var connection = _dataSource.OpenConnection();
        return connection.QueryFirst<User>(sql, new {userId = user.Id, username = user.Username, email = user.Email, firstname= user.Firstname, lastname = user.Lastname});
    }
    
    public User Delete(int id)
    {
        const string sql = $@"
DELETE FROM weight_tracker.users
WHERE id = @userId
RETURNING
    id AS {nameof(User.Id)},
    username AS {nameof(User.Username)},
    email AS {nameof(User.Email)},
    firstname AS {nameof(User.Firstname)},
    lastname AS {nameof(User.Lastname)}
;";

        using var connection = _dataSource.OpenConnection();
        return connection.QueryFirst<User>(sql, new {id});
    }
    
    
}