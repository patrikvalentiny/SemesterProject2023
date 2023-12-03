using System.Data.Common;
using Dapper;
using infrastructure.DataModels;

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

    public decimal? GetDifferenceBetweenLatestAndPreviousWeight(DateTime weightDate, int userId)
    {
       var sql = $@"SELECT (SELECT weight FROM weight_tracker.weights WHERE user_id = @user_id AND date < @date ORDER BY date DESC LIMIT 1) -
       (SELECT weight FROM weight_tracker.weights WHERE user_id = @user_id AND date = @date) AS {nameof(WeightInput.Difference)};";

        using var conn = dataSource.OpenConnection();
        return decimal.Round(conn.QueryFirst<decimal>(sql, new { user_id = userId, date = weightDate }), 2);
    }
}