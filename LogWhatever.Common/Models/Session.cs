using System;

namespace LogWhatever.Common.Models
{
	public class Session : BaseModel
	{
		#region Properties
		public User User { get; set; }
		#endregion
	}
}