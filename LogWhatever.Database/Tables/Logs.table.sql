create table Logs (
	Id uniqueidentifier not null primary key,
	UpdatedDate datetime not null,
	UserId uniqueidentifier not null,
	Name nvarchar(255) not null,
	Date datetime not null
)