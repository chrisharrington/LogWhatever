using System;

namespace LogWhatever.Common.Models
{
	public class MeasurementValue : BaseModel
	{
		#region Properties
		public Guid LogId { get; set; }
		public Guid UserId { get; set; }
		public Guid EventId { get; set; }
		public Guid MeasurementId { get; set; }
		public decimal Quantity { get; set; }
		#endregion
	}
}