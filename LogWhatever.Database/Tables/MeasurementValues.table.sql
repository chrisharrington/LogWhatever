create table MeasurementValues (
	Id uniqueidentifier not null primary key,
	UpdatedDate datetime not null,
	MeasurementId uniqueidentifier not null,
	Quantity decimal(18,4) not null
)