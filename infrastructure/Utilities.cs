namespace infrastructure;

public static class Utilities
{
    public static string ProperlyFormattedConnectionString(string envVarName)
    {
        Uri uri = new(Environment.GetEnvironmentVariable(envVarName)!);
        return string.Format(
            "Server={0};Database={1};User Id={2};Password={3};Port={4};Pooling=true;MaxPoolSize=3",
            uri.Host,
            uri.AbsolutePath.Trim('/'),
            uri.UserInfo.Split(':')[0],
            uri.UserInfo.Split(':')[1],
            uri.Port > 0 ? uri.Port : 5432);
    }
}