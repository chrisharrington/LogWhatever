create table TagEvents (
	Id uniqueidentifier not null primary key,
	UpdatedDate datetime not null,
	UserId uniqueidentifier not null,
	TagId uniqueidentifier not null,
	Name nvarchar(255) not null,
	EventId uniqueidentifier not null,
	LogId uniqueidentifier not null,
	LogName nvarchar(255) not null
)