using LogWhatever.Common.CQRS;
using LogWhatever.Common.Extensions;
using LogWhatever.Messages.Events;

namespace LogWhatever.Handlers.Denormalizers
{
	public class UserDenormalizer : BaseDenormalizer, IHandleMessagesOfType<UserAdded>
	{
		#region Public Methods
		public void Handle(UserAdded message)
		{
			using (var connection = OpenConnection())
			{
				connection.Insert("Users", message);
			}
		}
		#endregion
	}
}