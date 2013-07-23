using System;

namespace LogWhatever.Common.Models
{
	public class Measurement : BaseModel
	{
		#region Properties
		public Guid UserId { get; set; }
		public Guid GroupId { get; set; }
		public Guid LogId { get; set; }
		public string LogName { get; set; }
		public Guid EventId { get; set; }
		public string Name { get; set; }
		public decimal Quantity { get; set; }
		public string Unit { get; set; }
		#endregion
	}
}