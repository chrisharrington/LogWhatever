create table Tags (
	Id uniqueidentifier not null primary key,
	UpdatedDate datetime not null,
	UserId uniqueidentifier not null,
	LogId uniqueidentifier not null,
	LogName nvarchar(255) not null,
	EventId uniqueidentifier not null,
	EventName nvarchar(255) not null,
	Value nvarchar(255) not null
)