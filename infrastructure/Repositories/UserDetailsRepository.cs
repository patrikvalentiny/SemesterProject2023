using System.Data.Common;
using Dapper;
using infrastructure.DataModels;

namespace infrastructure.Repositories;

public class UserDetailsRepository(DbDataSource dataSource) : IRepository<UserDetails>
{
    public UserDetails? GetById(int id)
    {
        const string sql = $@"
SELECT
    user_id as {nameof(UserDetails.UserId)},
    height_cm as {nameof(UserDetails.Height)},
    target_weight_kg as {nameof(UserDetails.TargetWeight)},
    target_date as {nameof(UserDetails.TargetDate)},
    firstname as {nameof(UserDetails.Firstname)},
    lastname as {nameof(UserDetails.Lastname)}
FROM weight_tracker.user_details
WHERE user_id = @id;
";
        using var connection = dataSource.OpenConnection();
        return connection.QueryFirstOrDefault<UserDetails>(sql, new { id });
    }

    public IEnumerable<UserDetails> GetAll()
    {
        throw new NotImplementedException();
    }

    public UserDetails Create(UserDetails entity)
    {
        const string sql = $@"
           INSERT INTO weight_tracker.user_details (user_id, height_cm, target_weight_kg, target_date, firstname, lastname, loss_per_week) VALUES 
        (@{nameof(UserDetails.UserId)}, @{nameof(UserDetails.Height)}, @{nameof(UserDetails.TargetWeight)}, @{nameof(UserDetails.TargetDate)},
         @{nameof(UserDetails.Firstname)}, @{nameof(UserDetails.Lastname)}, @{nameof(UserDetails.LossPerWeek)})
           RETURNING
               user_id as {nameof(UserDetails.UserId)},
               height_cm as {nameof(UserDetails.Height)},
               target_weight_kg as {nameof(UserDetails.TargetWeight)},
               target_date as {nameof(UserDetails.TargetDate)},
               firstname as {nameof(UserDetails.Firstname)},
                lastname as {nameof(UserDetails.Lastname)},
                loss_per_week as {nameof(UserDetails.LossPerWeek)}
               ;";
        using var connection = dataSource.OpenConnection();
        return connection.QueryFirst<UserDetails>(sql, entity);
    }

    public UserDetails Update(UserDetails entity)
    {
        const string sql = $@"
              UPDATE weight_tracker.user_details 
              SET 
                  height_cm = @{nameof(UserDetails.Height)}, 
                   target_weight_kg = @{nameof(UserDetails.TargetWeight)}, 
                   target_date = @{nameof(UserDetails.TargetDate)},
                     firstname = @{nameof(UserDetails.Firstname)},
                        lastname = @{nameof(UserDetails.Lastname)}
                WHERE user_id = @{nameof(UserDetails.UserId)}
              RETURNING 
                    user_id as {nameof(UserDetails.UserId)},
                    height_cm as {nameof(UserDetails.Height)},
                    target_weight_kg as {nameof(UserDetails.TargetWeight)},
                    target_date as {nameof(UserDetails.TargetDate)}
                    ;
            ";
        using var connection = dataSource.OpenConnection();
        return connection.QueryFirst<UserDetails>(sql, entity);
    }

    public UserDetails Delete(int id)
    {
        throw new NotImplementedException();
    }
}