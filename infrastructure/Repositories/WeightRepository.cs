using System.Data.Common;
using Dapper;
using infrastructure.DataModels;

namespace infrastructure.Repositories;

public class WeightRepository(DbDataSource dataSource)
{
    public WeightInput Create(WeightInput entity)
    {
        var sql =
            $@"INSERT INTO weight_tracker.weights (weight, date, user_id, body_fat_percentage, skeletal_muscle_kg) 
VALUES (@weight, @date, @user_id, @bodyFat, @skeletalMuscle) ON CONFLICT (user_id, date) DO UPDATE SET weight = @weight, date = @date, body_fat_percentage = @bodyFat, skeletal_muscle_kg = @skeletalMuscle
RETURNING 
    weight as {nameof(WeightInput.Weight)}, 
    date as {nameof(WeightInput.Date)}, 
    user_id as {nameof(WeightInput.UserId)},
    body_fat_percentage as {nameof(WeightInput.BodyFatPercentage)},
    skeletal_muscle_kg as {nameof(WeightInput.SkeletalMuscleWeight)};";

        using var conn = dataSource.OpenConnection();
        return conn.QueryFirst<WeightInput>(sql,
            new
            {
                weight = entity.Weight, date = entity.Date, user_id = entity.UserId, bodyFat = entity.BodyFatPercentage,
                skeletalMuscle = entity.SkeletalMuscleWeight
            });
    }

    public WeightInput Update(WeightInput entity)
    {
        var sql =
            $@"UPDATE weight_tracker.weights SET weight = @weight,body_fat_percentage = @bodyFat, skeletal_muscle_kg = @skeletalMuscle WHERE date = @date AND user_id = @user_id RETURNING
    weight as {nameof(WeightInput.Weight)}, 
    date as {nameof(WeightInput.Date)}, 
    user_id as {nameof(WeightInput.UserId)},
    body_fat_percentage as {nameof(WeightInput.BodyFatPercentage)},
    skeletal_muscle_kg as {nameof(WeightInput.SkeletalMuscleWeight)};";

        using var conn = dataSource.OpenConnection();
        return conn.QueryFirst<WeightInput>(sql,
            new
            {
                weight = entity.Weight, date = entity.Date, user_id = entity.UserId, bodyFat = entity.BodyFatPercentage,
                skeletalMuscle = entity.SkeletalMuscleWeight
            });
    }

    public WeightInput Delete(DateTime date, int userId)
    {
        var sql = $@"DELETE FROM weight_tracker.weights WHERE date = @date AND user_id = @user_id RETURNING 
    weight as {nameof(WeightInput.Weight)}, 
    date as {nameof(WeightInput.Date)}, 
    user_id as {nameof(WeightInput.UserId)},
    body_fat_percentage as {nameof(WeightInput.BodyFatPercentage)},
    skeletal_muscle_kg as {nameof(WeightInput.SkeletalMuscleWeight)}";
        using var conn = dataSource.OpenConnection();
        return conn.QueryFirst<WeightInput>(sql, new { date, user_id = userId });
    }

    public IEnumerable<WeightInput> GetAllWeightsForUser(int dataUserId)
    {
        var sql = $@"SELECT
    w.weight as {nameof(WeightInput.Weight)}, 
    w.date as {nameof(WeightInput.Date)}, 
    w.user_id as {nameof(WeightInput.UserId)},
    w.body_fat_percentage as {nameof(WeightInput.BodyFatPercentage)},
    w.skeletal_muscle_kg as {nameof(WeightInput.SkeletalMuscleWeight)}
FROM weight_tracker.weights w
LEFT JOIN weight_tracker.journeys j on w.user_id = j.user_id
WHERE w.user_id = @user_id AND j.selected IS NULL OR (j.selected AND date BETWEEN j.start_date and j.end_date)  ORDER BY date;";

        using var conn = dataSource.OpenConnection();
        return conn.Query<WeightInput>(sql, new { user_id = dataUserId });
    }

    public WeightInput? GetLatestWeightForUser(int userId)
    {
        var sql = $@"SELECT
    weight as {nameof(WeightInput.Weight)}, 
    date as {nameof(WeightInput.Date)}, 
    user_id as {nameof(WeightInput.UserId)},
    body_fat_percentage as {nameof(WeightInput.BodyFatPercentage)},
    skeletal_muscle_kg as {nameof(WeightInput.SkeletalMuscleWeight)}
FROM weight_tracker.weights WHERE user_id = @user_id ORDER BY date DESC LIMIT 1;";

        using var conn = dataSource.OpenConnection();
        return conn.QueryFirstOrDefault<WeightInput>(sql, new { user_id = userId });
    }
}