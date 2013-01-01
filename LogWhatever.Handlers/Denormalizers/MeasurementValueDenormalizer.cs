using System;
using LogWhatever.Common.CQRS;
using LogWhatever.Common.Extensions;
using LogWhatever.Messages.Events;

namespace LogWhatever.Handlers.Denormalizers
{
	public class MeasurementValueDenormalizer : BaseDenormalizer, IHandleMessagesOfType<MeasurementValueAdded>
	{
		#region Public Methods
		public void Handle(MeasurementValueAdded message)
		{
			if (message == null)
				throw new ArgumentNullException("message");

			using (var connection = OpenConnection())
			{
				connection.Insert("MeasurementValues", message);
			}
		}
		#endregion
	}
}