using System;
using LogWhatever.Common.Models;

namespace LogWhatever.Messages.Commands
{
	public class AddMeasurementValue : BaseCommand
	{
		#region Properties
		public Guid LogId { get; set; }
		public Guid UserId { get; set; }
		public Guid MeasurementId { get; set; }
		public decimal Quantity { get; set; }
		public Guid EventId { get; set; }
		#endregion

		#region Public Methods
		public static AddMeasurementValue CreateFrom(MeasurementValue value)
		{
			return new AddMeasurementValue {
				Id = value.Id,
				LogId = value.LogId,
				UserId = value.UserId,
				MeasurementId = value.MeasurementId,
				Quantity = value.Quantity,
				UpdatedDate = value.UpdatedDate,
				EventId = value.EventId
			};
		}
		#endregion
	}
}