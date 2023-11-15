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
        throw new NotImplementedException();
    }

    public WeightInput Delete(int id)
    {
        throw new NotImplementedException();
    }
}