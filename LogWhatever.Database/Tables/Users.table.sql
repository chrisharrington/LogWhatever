create table Users (
	Id uniqueidentifier not null primary key,
	Name nvarchar(255) not null,
	EmailAddress nvarchar(255) not null
)