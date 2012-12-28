using LogWhatever.Common.CQRS;
using LogWhatever.Common.Extensions;
using LogWhatever.Messages.Events;

namespace LogWhatever.Handlers.Denormalizers
{
	public class EventDenormalizer : BaseDenormalizer, IHandleMessagesOfType<EventAdded>
	{
		#region Public Methods
		public void Handle(EventAdded message)
		{
			using (var connection = OpenConnection())
			{
				connection.Insert("Events", message);
			}
		}
		#endregion
	}
}