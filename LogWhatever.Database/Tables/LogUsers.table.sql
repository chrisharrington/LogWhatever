﻿create table LogUsers (
	Id uniqueidentifier not null primary key,
	UpdatedDate datetime not null,
	Name nvarchar(255) not null,
	EmailAddress nvarchar(255) not null
)