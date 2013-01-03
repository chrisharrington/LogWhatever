using LogWhatever.Common.CQRS;
using LogWhatever.Messages.Commands;

namespace LogWhatever.Handlers.Commands
{
	public class UserHandler : BaseCommandHandler, IHandleMessagesOfType<AddUser>
	{
		#region Public Methods
		public void Handle(AddUser message)
		{
			CreateAndDispatchEvent(message);
		}
		#endregion
	}
}