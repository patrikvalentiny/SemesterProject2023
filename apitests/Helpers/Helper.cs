using System.Data.Common;
using Dapper;
using infrastructure;
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
    date    timestamp not null,
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

    public static void TriggerRebuild()
    {
        using var conn = DataSource.OpenConnection();
        try
        {
            conn.Execute(RebuildScript);
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

    public static void InsertUser1()
    {
        const string sql =
            "INSERT INTO weight_tracker.users (id, username, email) VALUES (1, 'testUserHelper', 'testUserHelper@test.test')";
        using var conn = Helper.OpenConnection();
        conn.Execute(sql);
    }
}