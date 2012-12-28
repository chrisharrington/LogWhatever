using LogWhatever.Common.CQRS;
using LogWhatever.Messages.Commands;

namespace LogWhatever.Handlers.Commands
{
	public class EventHandler : BaseCommandHandler, IHandleMessagesOfType<AddEvent>
	{
		#region Public Methods
		public void Handle(AddEvent message)
		{
			CreateAndDispatchEvent(message);
		}
		#endregion
	}
}