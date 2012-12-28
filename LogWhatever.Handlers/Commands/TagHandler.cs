using LogWhatever.Common.CQRS;
using LogWhatever.Messages.Commands;

namespace LogWhatever.Handlers.Commands
{
	public class TagHandler : BaseCommandHandler, IHandleMessagesOfType<AddTag>
	{
		#region Public Methods
		public void Handle(AddTag message)
		{
			CreateAndDispatchEvent(message);
		}
		#endregion
	}
}