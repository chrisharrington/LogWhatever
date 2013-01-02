create table Tags (
	Id uniqueidentifier not null primary key,
	Name nvarchar(255) null,
	UpdatedDate datetime not null
)