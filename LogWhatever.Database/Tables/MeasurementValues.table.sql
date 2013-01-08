create table MeasurementValues (
	Id uniqueidentifier not null primary key,
	UserId uniqueidentifier not null,
	LogId uniqueidentifier not null,
	EventId uniqueidentifier not null,
	UpdatedDate datetime not null,
	MeasurementId uniqueidentifier not null,
	Quantity decimal(18,4) not null
)