using System.Data.Common;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using infrastructure;
using Konscious.Security.Cryptography;
using Npgsql;

namespace apitests;

public static class Helper
{
    public static int UserId => 1;
    private static readonly DbDataSource DataSource;

    private static readonly string RebuildScript = @"
drop schema if exists weight_tracker cascade;
create schema weight_tracker;
create table weight_tracker.users
(
    id       serial
        primary key,
    username varchar(64) not null
        constraint username_uk
            unique,
    email    varchar(128)
        constraint email_uk
            unique
);

create table weight_tracker.passwords
(
    user_id       integer     not null
        constraint passwords_users_userid_fk
            references weight_tracker.users
            on update cascade on delete cascade,
    password_hash varchar     not null,
    salt          varchar     not null,
    algorithm     varchar(64) not null
);

create table weight_tracker.weights
(
    weight  real      not null,
    date    date not null,
    user_id integer   not null
        constraint user_weights_users_id_fk
            references weight_tracker.users
            on update cascade on delete cascade,
    constraint weights_pk
        primary key (user_id, date)
);

create table weight_tracker.user_details
(
    user_id          integer
        constraint user_details_users_id_fk
            references weight_tracker.users
            on update cascade on delete cascade,
    height_cm        integer,
    target_weight_kg real,
    target_date      date,
    firstname        varchar(128),
    lastname         varchar(128),
    loss_per_week    real
);
";

    static Helper()
    {
        DataSource =
            new NpgsqlDataSourceBuilder(Utilities.FormatConnectionString(
                Environment.GetEnvironmentVariable("ASPNETCORE_ConnectionStrings__WebApiDatabase")!)).Build();
        DataSource.OpenConnection().Close();
    }

    public static async Task TriggerRebuild()
    {
        await using var conn = DataSource.OpenConnection();
        try
        {
            await conn.ExecuteAsync(RebuildScript);
        }
        catch (Exception e)
        {
            throw new Exception(@"THERE WAS AN ERROR REBUILDING THE DATABASE", e);
        }
    }

    public static DbConnection OpenConnection()
    {
        return DataSource.OpenConnection();
    }

    public static string? GetToken()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_TestJwt");
    }

    public static readonly User User1 = new()
    {
        Id = 1,
        Username = "testUserHelper",
        Email = "testUserHelper@test.test",
    };
    
    public static readonly string UserPassword = "testPasswordHelper";
    
    public static async Task InsertUser1()
    {
        var sql =
            "INSERT INTO weight_tracker.users (id, username, email) VALUES (@Id, @Username, @Email)";
        
        await using var conn = OpenConnection();
        try
        {
            await conn.ExecuteAsync(sql, User1);
        }
        catch (Exception e)
        {
            throw new Exception(@"THERE WAS AN ERROR INSERTING USER 1", e);
        }
        sql =
            "insert into weight_tracker.passwords (user_id, password_hash, salt, algorithm) values (@Id, @Password, @Salt, 'argon2id')";
        using var hashAlgo = new Argon2id(Encoding.UTF8.GetBytes(UserPassword));
        hashAlgo.Salt = RandomNumberGenerator.GetBytes(128);
        hashAlgo.MemorySize = 12288;
        hashAlgo.Iterations = 3;
        hashAlgo.DegreeOfParallelism = 1;
        try { await conn.ExecuteAsync(sql, new
        {
            Id = User1.Id,
            Password = Convert.ToBase64String(await hashAlgo.GetBytesAsync(256)),
            Salt = Convert.ToBase64String(hashAlgo.Salt)
        }); }
        catch (Exception e)
        {
            throw new Exception(@"THERE WAS AN ERROR INSERTING USER 1 PASSWORD", e);
        }
        await conn.CloseAsync();
    }
}