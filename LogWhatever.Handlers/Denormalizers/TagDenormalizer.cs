using LogWhatever.Common.CQRS;
using LogWhatever.Common.Extensions;
using LogWhatever.Messages.Events;

namespace LogWhatever.Handlers.Denormalizers
{
	public class TagDenormalizer : BaseDenormalizer, IHandleMessagesOfType<TagAdded>
	{
		#region Public Methods
		public void Handle(TagAdded message)
		{
			using (var connection = OpenConnection())
			{
				connection.Insert("Tags", message);
			}
		}
		#endregion
	}
}