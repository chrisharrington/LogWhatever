using LogWhatever.Common.CQRS;
using LogWhatever.Messages.Commands;

namespace LogWhatever.Handlers.Commands
{
	public class MeasurementHandler : BaseCommandHandler, IHandleMessagesOfType<AddMeasurement>
	{
		#region Public Methods
		public void Handle(AddMeasurement message)
		{
			CreateAndDispatchEvent(message);
		}
		#endregion
	}
}