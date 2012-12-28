using System;
using LogWhatever.Common.Models;

namespace LogWhatever.Messages.Commands
{
	public class AddEvent : BaseCommand
	{
		#region Properties
		public Guid UserId { get; set; }
		public Guid LogId { get; set; }
		public string LogName { get; set; }
		public DateTime Date { get; set; }
		#endregion

		#region Public Methods
		public static AddEvent CreateFrom(Event @event)
		{
			return new AddEvent {
				Date = @event.Date,
				Id = @event.Id,
				LogId = @event.LogId,
				LogName = @event.LogName,
				UpdatedDate = @event.UpdatedDate,
				UserId = @event.UserId
			};
		}
		#endregion
	}
}