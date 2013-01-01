using System;

namespace LogWhatever.Common.Models
{
	public class MeasurementValue : BaseModel
	{
		#region Properties
		public Guid MeasurementId { get; set; }
		public decimal Quantity { get; set; }
		#endregion
	}
}