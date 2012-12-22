using System;

namespace LogWhatever.Common.Models
{
	public class Event : BaseModel
	{
		#region Properties
		public Guid UserId { get; set; }
		public Guid LogId { get; set; }
		public string LogName { get; set; }
		public DateTime Date { get; set; }
		#endregion
	}
}