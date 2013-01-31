using System;

namespace LogWhatever.Common.Models
{
	public class Session : BaseModel
	{
		#region Data Members
		private User _user;
		#endregion

		#region Properties
		public Guid UserId { get; set; }
		public string Name { get; set; }
		public string EmailAddress { get; set; }
		
		public User User
		{
			get { return _user = new User {Id = UserId, Name = Name, EmailAddress = EmailAddress}; }
		}
		#endregion
	}
}