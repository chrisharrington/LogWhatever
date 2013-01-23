using System;
using LogWhatever.Common.Models;

namespace LogWhatever.Messages.Commands
{
	public class AddMeasurement : BaseCommand
	{
		#region Properties
		public string Name { get; set; }
		public Guid UserId { get; set; }
		public Guid GroupId { get; set; }
		public Guid LogId { get; set; }
		public string LogName { get; set; }
		public Guid EventId { get; set; }
		public decimal Quantity { get; set; }
		public string Unit { get; set; }
		public DateTime Date { get; set; }
		#endregion

		#region Public Methods
		public static AddMeasurement CreateFrom(Measurement measurement)
		{
			return new AddMeasurement {
				EventId = measurement.EventId,
				Id = measurement.Id,
				GroupId = measurement.GroupId,
				LogId = measurement.LogId,
				LogName = measurement.LogName,
				Name = measurement.Name,
				Quantity = measurement.Quantity,
				Unit = measurement.Unit,
				UpdatedDate = measurement.UpdatedDate,
				UserId = measurement.UserId,
				Date = measurement.Date
			};
		}
		#endregion
	}
}