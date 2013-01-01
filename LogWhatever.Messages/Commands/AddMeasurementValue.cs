using System;
using LogWhatever.Common.Models;

namespace LogWhatever.Messages.Commands
{
	public class AddMeasurementValue : BaseCommand
	{
		#region Properties
		public Guid MeasurementId { get; set; }
		public decimal Quantity { get; set; }
		#endregion

		#region Public Methods
		public static AddMeasurementValue CreateFrom(MeasurementValue value)
		{
			return new AddMeasurementValue {
				Id = value.Id,
				MeasurementId = value.MeasurementId,
				Quantity = value.Quantity,
				UpdatedDate = value.UpdatedDate
			};
		}
		#endregion
	}
}