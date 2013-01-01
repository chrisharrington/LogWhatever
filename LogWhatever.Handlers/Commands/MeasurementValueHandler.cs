using LogWhatever.Common.CQRS;
using LogWhatever.Messages.Commands;

namespace LogWhatever.Handlers.Commands
{
	public class MeasurementValueHandler : BaseCommandHandler, IHandleMessagesOfType<AddMeasurementValue>
	{
		#region Public Methods
		public void Handle(AddMeasurementValue message)
		{
			CreateAndDispatchEvent(message);
		}
		#endregion
	}
}