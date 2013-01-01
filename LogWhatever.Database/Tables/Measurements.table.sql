create table Measurements (
	Id uniqueidentifier not null primary key,
	UpdatedDate datetime not null,
	UserId uniqueidentifier not null,
	LogId uniqueidentifier not null,
	LogName nvarchar(255) not null,
	EventId uniqueidentifier not null,
	Name nvarchar(255) not null,
	Unit nvarchar(255) not null
)