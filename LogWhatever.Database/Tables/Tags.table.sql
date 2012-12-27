create table Tags (
	Id uniqueidentifier not null primary key,
	Name nvarchar(255) null,
	UpdatedDate datetime not null,
	UserId uniqueidentifier not null,
	LogId uniqueidentifier not null,
	LogName nvarchar(255) not null,
	EventId uniqueidentifier not null,
	Value nvarchar(255) not null
)