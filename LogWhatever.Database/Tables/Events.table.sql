create table Events (
	Id uniqueidentifier not null primary key,
	UpdatedDate datetime not null,
	UserId uniqueidentifier not null,
	LogId uniqueidentifier not null,
	LogName nvarchar(255) not null,
	Date datetime not null
)