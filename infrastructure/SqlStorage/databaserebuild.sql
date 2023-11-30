drop schema if exists weight_tracker cascade;
create schema weight_tracker;
create table weight_tracker.users
(
    id        SERIAL
        primary key,
    username  varchar(64)  not null,
    email     varchar(128) not null
        constraint users_pk2
            unique,
    firstname varchar(128),
    lastname  varchar(128)
);

alter table weight_tracker.passwords
    owner to flzosjpj;
-- alter table weight_tracker.users
--     owner to wiiyhvmb;

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

alter table weight_tracker.passwords
    owner to flzosjpj;
-- alter table weight_tracker.passwords
--     owner to wiiyhvmb;
