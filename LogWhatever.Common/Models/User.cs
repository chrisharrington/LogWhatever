using System;

namespace LogWhatever.Common.Models
{
	public class User : BaseModel
	{
		#region Properties
		public string EmailAddress { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }
		#endregion
	}
}