﻿using LogWhatever.Common.CQRS;
using LogWhatever.Messages.Commands;

namespace LogWhatever.Handlers.Commands
{
	public class AddLogHandler : BaseCommandHandler, IHandleMessagesOfType<AddLog>
	{
		#region Public Methods
		public void Handle(AddLog message)
		{
			CreateAndDispatchEvent(message);
		}
		#endregion
	}
}