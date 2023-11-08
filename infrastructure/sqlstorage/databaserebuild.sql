drop schema weight_tracker cascade;
create schema weight_tracker;
create table weight_tracker.users
(
    userid    SERIAL
        primary key,
    username  varchar(64) not null,
    email     integer
        constraint users_pk2
            unique,
    firstname varchar(128),
    lastname  varchar(128)
);

alter table weight_tracker.users
    owner to flzosjpj;

create table weight_tracker.passwords
(
    userid        integer     not null
        constraint passwords_users_userid_fk
            references weight_tracker.users
            on update cascade on delete cascade,
    password_hash varchar     not null,
    salt          varchar     not null,
    algorithm     varchar(64) not null
);

alter table weight_tracker.passwords
    owner to flzosjpj;

