using LogWhatever.Common.Models;

namespace LogWhatever.Messages.Commands
{
	public class AddUser : BaseCommand
	{
		#region Properties
		public string EmailAddress { get; set; }
		public string Name { get; set; }
		#endregion

		#region Public Methods
		public static AddUser CreateFrom(User user)
		{
			return new AddUser {
				EmailAddress = user.EmailAddress,
				Id = user.Id,
				Name = user.Name,
				UpdatedDate = user.UpdatedDate
			};
		}
		#endregion
	}
}