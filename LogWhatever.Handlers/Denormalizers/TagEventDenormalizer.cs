using LogWhatever.Common.CQRS;
using LogWhatever.Common.Extensions;
using LogWhatever.Messages.Events;

namespace LogWhatever.Handlers.Denormalizers
{
	public class TagEventDenormalizer : BaseDenormalizer, IHandleMessagesOfType<TagEventAdded>
	{
		#region Public Methods
		public void Handle(TagEventAdded message)
		{
			using (var connection = OpenConnection())
			{
				connection.Insert("TagEvents", message);
			}
		}
		#endregion
	}
}