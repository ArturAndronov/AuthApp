create table Users (
	Id serial primary key,
	Username varchar(255) not null,
	Password varchar(255) not null,
	Email varchar(255) not null
	);

create index idx_username on Users(Username);
create index idx_email on Users(Email);