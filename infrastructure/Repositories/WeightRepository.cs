using System.Data;
using System.Data.Common;
using Dapper;
using infrastructure.DataModels;
using Microsoft.AspNetCore.Components.Web;
using Serilog;

namespace infrastructure.Repositories;

public class WeightRepository(DbDataSource dataSource)
{
    public WeightInput Create(WeightInput entity)
    {
        var sql = $@"INSERT INTO weight_tracker.weights (weight, date, user_id) 
VALUES (@weight, @date, @user_id) ON CONFLICT (user_id, date) DO UPDATE SET weight = @weight, date = @date
RETURNING 
    weight as {nameof(WeightInput.Weight)}, 
    date as {nameof(WeightInput.Date)}, 
    user_id as {nameof(WeightInput.UserId)};";

        using var conn = dataSource.OpenConnection();
        return conn.QueryFirst<WeightInput>(sql, new { weight = entity.Weight, date = entity.Date, user_id = entity.UserId });
    }

    public WeightInput Update(WeightInput entity)
    {
        var sql =
            $@"UPDATE weight_tracker.weights SET weight = @weight WHERE date = @date AND user_id = @user_id RETURNING
    weight as {nameof(WeightInput.Weight)}, 
    date as {nameof(WeightInput.Date)}, 
    user_id as {nameof(WeightInput.UserId)};";

        using var conn = dataSource.OpenConnection();
        return conn.QueryFirst<WeightInput>(sql,
            new { weight = entity.Weight, date = entity.Date, user_id = entity.UserId });
    }

    public WeightInput Delete(DateTime date, int userId)
    {
        var sql = $@"DELETE FROM weight_tracker.weights WHERE date = @date AND user_id = @user_id RETURNING 
    weight as {nameof(WeightInput.Weight)}, 
    date as {nameof(WeightInput.Date)}, 
    user_id as {nameof(WeightInput.UserId)};";
        using var conn = dataSource.OpenConnection();
        return conn.QueryFirst<WeightInput>(sql, new { date, user_id = userId });
    }

    public IEnumerable<WeightInput> GetAllWeightsForUser(int dataUserId)
    {
        var sql = $@"SELECT
    weight as {nameof(WeightInput.Weight)}, 
    date as {nameof(WeightInput.Date)}, 
    user_id as {nameof(WeightInput.UserId)}
FROM weight_tracker.weights WHERE user_id = @user_id ORDER BY date;";

        using var conn = dataSource.OpenConnection();
        return conn.Query<WeightInput>(sql, new { user_id = dataUserId });
    }

    public WeightInput? GetLatestWeightForUser(int userId)
    {
        var sql = $@"SELECT
    weight as {nameof(WeightInput.Weight)}, 
    date as {nameof(WeightInput.Date)}, 
    user_id as {nameof(WeightInput.UserId)}
FROM weight_tracker.weights WHERE user_id = @user_id ORDER BY date DESC LIMIT 1;";

        using var conn = dataSource.OpenConnection();
        return conn.QueryFirstOrDefault<WeightInput>(sql, new { user_id = userId });
    }
}