using System.Data;

namespace apitests.Helpers;

public interface IDataSource
{
    public string ConnectionString { get; }
    public IDbConnection OpenConnection();
}