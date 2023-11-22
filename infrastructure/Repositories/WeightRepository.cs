using System.Data.Common;
using Dapper;
using infrastructure.DataModels;

namespace infrastructure.Repositories;

public class WeightRepository(DbDataSource dataSource)
{
    public WeightInput GetById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<WeightInput> GetAll()
    {
        throw new NotImplementedException();
    }

    public WeightInput Create(decimal weight, DateTime date, int userId)
    {
        var sql = $@"INSERT INTO weight_tracker.weights (weight, date, user_id) 
VALUES (@weight, @date, @user_id) 
RETURNING 
    id as {nameof(WeightInput.Id)}, 
    weight as {nameof(WeightInput.Weight)}, 
    date as {nameof(WeightInput.Date)}, 
    user_id as {nameof(WeightInput.UserId)};";

        using var conn = dataSource.OpenConnection();
        return conn.QueryFirst<WeightInput>(sql, new { weight, date, user_id = userId });
    }

    public WeightInput Update(WeightInput entity)
    {
        var sql = $@"UPDATE weight_tracker.weights SET weight = @weight, date = @date WHERE id = @id AND user_id = @user_id RETURNING
                                                                 id as {nameof(WeightInput.Id)}, 
    weight as {nameof(WeightInput.Weight)}, 
    date as {nameof(WeightInput.Date)}, 
    user_id as {nameof(WeightInput.UserId)};";
            
        using var conn = dataSource.OpenConnection();
        return conn.QueryFirst<WeightInput>(sql, new { weight = entity.Weight, date = entity.Date, user_id = entity.UserId, id = entity.Id });
    }

    public WeightInput Delete(int id, int userId)
    {
        var sql = $@"DELETE FROM weight_tracker.weights WHERE id = @id AND user_id = @user_id RETURNING 
id as {nameof(WeightInput.Id)}, 
    weight as {nameof(WeightInput.Weight)}, 
    date as {nameof(WeightInput.Date)}, 
    user_id as {nameof(WeightInput.UserId)};";
        using var conn = dataSource.OpenConnection();
        return conn.QueryFirst<WeightInput>(sql, new { id, user_id = userId });
    }

    public IEnumerable<WeightInput> GetAllWeightsForUser(int dataUserId)
    {
        var sql = $@"SELECT
id as {nameof(WeightInput.Id)}, 
    weight as {nameof(WeightInput.Weight)}, 
    date as {nameof(WeightInput.Date)}, 
    user_id as {nameof(WeightInput.UserId)}
FROM weight_tracker.weights WHERE user_id = @user_id ORDER BY date DESC;";

        using var conn = dataSource.OpenConnection();
        return conn.Query<WeightInput>(sql, new { user_id = dataUserId });
    }

    public WeightInput? GetLatestWeightForUser(int userId)
    {
        var sql = $@"SELECT
id as {nameof(WeightInput.Id)}, 
    weight as {nameof(WeightInput.Weight)}, 
    date as {nameof(WeightInput.Date)}, 
    user_id as {nameof(WeightInput.UserId)}
FROM weight_tracker.weights WHERE user_id = @user_id ORDER BY date DESC LIMIT 1;";

        using var conn = dataSource.OpenConnection();
        return conn.QueryFirstOrDefault<WeightInput>(sql, new { user_id = userId });
    }
}