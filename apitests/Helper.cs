using System.Configuration;
using System.Data.Common;
using System.Reflection;
using Dapper;
using infrastructure;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace apitests;

public static class Helper
{
    private static readonly DbDataSource DataSource;
    private static readonly string RebuildScript = $@"
drop schema if exists weight_tracker cascade;
create schema weight_tracker;
create table weight_tracker.users
(
    id        serial
        primary key,
    username  varchar(64)  not null,
    email     varchar(128) not null
        constraint users_pk2
            unique,
    firstname varchar(128),
    lastname  varchar(128)
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

";

    static Helper()
    {
        DataSource =
        new NpgsqlDataSourceBuilder(Utilities.FormatConnectionString(Environment.GetEnvironmentVariable("WT_DB")!)).Build();
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
}