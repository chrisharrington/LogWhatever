using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.Web.Controllers.Api
{
	public class LogsController : AuthorizedController
	{
		#region Properties
		public ILogRepository LogRepository { get; set; }
		public IMeasurementRepository MeasurementRepository { get; set; }
		public ITagRepository TagRepository { get; set; }
		public IEventRepository EventRepository { get; set; }
		#endregion

		#region Public Methods
		public void Post(LogData data)
		{
			data.User = GetCurrentSession().User;
			var log = GetLog(data);
			var @event = CreateEvent(data, log);
			SaveMeasurements(data.User, log, @event, data.Measurements);
			SaveTags(log, data.User, @event, data.Tags);
		}
		#endregion

		#region Private Methods
		private void SaveTags(Log log, User user, Event @event, IEnumerable<Tag> tags)
		{
			foreach (var tag in tags)
				TagRepository.Create(new Tag { LogId = log.Id, LogName = log.Name, EventId = @event.Id, Date = @event.Date, Id = Guid.NewGuid(), Name = tag.Name, UserId = user.Id });
		}

		private void SaveMeasurements(User user, Log log, Event @event, IEnumerable<MeasurementData> values)
		{
			foreach (var value in values)
				MeasurementRepository.Create(new Measurement { Id = Guid.NewGuid(), GroupId = value.GroupId == Guid.Empty ? Guid.NewGuid() : value.GroupId, EventId = @event.Id, Date = @event.Date, Name = value.Name, Quantity = value.Quantity, Unit = value.Unit, LogId = log.Id, LogName = log.Name, UserId = user.Id });
		}

		private Event CreateEvent(LogData data, Log log)
		{
			var @event = new Event { Date = MergeDateAndTime(data.Date, data.Time), Id = Guid.NewGuid(), LogId = log.Id, LogName = log.Name, UserId = data.User.Id };
			EventRepository.Create(@event);
			return @event;
		}

		private Log GetLog(LogData data)
		{
			var log = LogRepository.Name(data.Name);
			if (log == null)
			{
				log = new Log { Id = Guid.NewGuid(), Name = data.Name, UserId = data.User.Id };
				LogRepository.Create(log);
			}
			return log;
		}

		private DateTime MergeDateAndTime(DateTime date, DateTime time)
		{
			return date.ToUniversalTime().AddHours(time.Hour).AddMinutes(time.Minute).AddSeconds(time.Second).AddMilliseconds(time.Millisecond);
		}
		#endregion

		#region LogData Class
		public class LogData
		{
			#region Properties
			public User User { get; set; }
			public string Name { get; set; }
			public DateTime Date { get; set; }
			public DateTime Time { get; set; }
			public IEnumerable<MeasurementData> Measurements { get; set; }
			public IEnumerable<Tag> Tags { get; set; }
			#endregion

			#region Constructors
			public LogData()
			{
				Tags = new List<Tag>();
				Measurements = new List<MeasurementData>();
			}
			#endregion
		}

		public class MeasurementData
		{
			#region Properties
			public string Name { get; set; }
			public decimal Quantity { get; set; }
			public string Unit { get; set; }
			public Guid GroupId { get; set; }
			#endregion
		}
		#endregion
	}
}