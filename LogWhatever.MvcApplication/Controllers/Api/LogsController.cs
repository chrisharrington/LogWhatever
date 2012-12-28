using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.Messages.Commands;

namespace LogWhatever.MvcApplication.Controllers.Api
{
	public class LogsController : AuthorizedController
	{
		#region Properties
		public ILogRepository LogRepository { get; set; }
		#endregion

		#region Public Methods
		public void Post(LogData data)
		{
			var log = GetLog(data);
			var @event = CreateEvent(data, log);
		}

		private Event CreateEvent(LogData data, Log log)
		{
			var @event =  new Event {
				Date = MergeDateAndTime(data.Date, data.Time),
				Id = Guid.NewGuid(),
				LogId = log.Id,
				LogName = log.Name,
				UserId = GetCurrentlySignedInUser().Id
			};
			Dispatcher.Dispatch(AddEvent.CreateFrom(@event));
			return @event;
		}

		private Log GetLog(LogData data)
		{
			var log = LogRepository.Name(data.Name);
			if (log == null)
			{
				log = new Log {Id = Guid.NewGuid(), Name = data.Name, UserId = GetCurrentlySignedInUser().Id};
				Dispatcher.Dispatch(AddLog.CreateFrom(log));
			}
			return log;
		}

		private DateTime MergeDateAndTime(DateTime date, DateTime time)
		{
			return date.AddHours(time.Hour).AddMinutes(time.Minute).AddSeconds(time.Second).AddMilliseconds(time.Millisecond);
		}
		#endregion

		#region LogData Class
		public class LogData
		{
			#region Properties
			public string Name { get; set; }
			public DateTime Date { get; set; }
			public DateTime Time { get; set; }
			public IEnumerable<Measurement> Measurements { get; set; }
			public IEnumerable<Tag> Tags { get; set; } 
			#endregion
		}
		#endregion
	}
}