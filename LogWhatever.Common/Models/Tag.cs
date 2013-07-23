using System;

namespace LogWhatever.Common.Models
{
	public class Tag : BaseModel
	{
		#region Properties
		public Guid UserId { get; set; }
		public Guid EventId { get; set; }
		public string Name { get; set; }
		public Guid LogId { get; set; }
		public string LogName { get; set; }
		#endregion
	}
}