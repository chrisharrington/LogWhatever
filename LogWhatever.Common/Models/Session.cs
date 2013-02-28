using System;

namespace LogWhatever.Common.Models
{
	public class Session : BaseModel
	{
		#region Properties
		public Guid UserId { get; set; }
		public string Name { get; set; }
		public string EmailAddress { get; set; }
		
		public User User
		{
			get { return new User {Id = UserId, Name = Name, EmailAddress = EmailAddress}; }
		}
		#endregion
	}
}