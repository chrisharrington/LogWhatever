using LogWhatever.Common.CQRS;
using LogWhatever.Common.Extensions;
using LogWhatever.Messages.Events;

namespace LogWhatever.Handlers.Denormalizers
{
	public class LogAddedHandler : BaseDenormalizer, IHandleMessagesOfType<LogAdded>
	{
		#region Public Methods
		public void Handle(LogAdded message)
		{
			using (var connection = OpenConnection())
			{
				connection.Insert("Logs", new {message.Id, message.UpdatedDate, message.Name, message.UserId});
			}
		}
		#endregion
	}
}