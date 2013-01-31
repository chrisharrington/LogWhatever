using LogWhatever.Common.CQRS;
using LogWhatever.Messages.Commands;

namespace LogWhatever.Handlers.Commands
{
	public class SessionHandler : BaseCommandHandler, IHandleMessagesOfType<AddSession>, IHandleMessagesOfType<DeleteSession>
	{
		#region Public Methods
		public void Handle(AddSession message)
		{
			CreateAndDispatchEvent(message);
		}

		public void Handle(DeleteSession message)
		{
			CreateAndDispatchEvent(message);
		}
		#endregion
	}
}