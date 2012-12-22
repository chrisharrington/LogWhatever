using System;

namespace LogWhatever.Common.Models
{
	public class Log : BaseModel
	{
		#region Properties
		public Guid UserId { get; set; }
		public string Name { get; set; }
		#endregion
	}
}