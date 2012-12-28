using LogWhatever.Common.CQRS;
using LogWhatever.Common.Extensions;
using LogWhatever.Messages.Events;

namespace LogWhatever.Handlers.Denormalizers
{
	public class MeasurementDenormalizer : BaseDenormalizer, IHandleMessagesOfType<MeasurementAdded>
	{
		#region Public Methods
		public void Handle(MeasurementAdded message)
		{
			using (var connection = OpenConnection())
			{
				connection.Insert("Measurements", message);
			}
		}
		#endregion
	}
}