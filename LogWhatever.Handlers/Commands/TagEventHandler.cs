using LogWhatever.Common.CQRS;
using LogWhatever.Messages.Commands;

namespace LogWhatever.Handlers.Commands
{
	public class TagEventHandler : BaseCommandHandler, IHandleMessagesOfType<AddTagEvent>
	{
		#region Public Methods
		public void Handle(AddTagEvent message)
		{
			CreateAndDispatchEvent(message);
		}
		#endregion
	}
}