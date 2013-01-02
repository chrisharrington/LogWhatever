using System;
using System.Collections.Generic;
using System.Linq;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.Messages.Commands;

namespace LogWhatever.MvcApplication.Controllers.Api
{
	public class LogsController : AuthorizedController
	{
		#region Properties
		public ILogRepository LogRepository { get; set; }
		public IMeasurementRepository MeasurementRepository { get; set; }
		public ITagRepository TagRepository { get; set; }
		#endregion

		#region Public Methods
		public void Post(LogData data)
		{
			data.User = GetCurrentlySignedInUser();
			var log = GetLog(data);
			var @event = CreateEvent(data, log);
			SaveMeasurements(data.User, log, @event, data.Measurements);
			SaveTags(log, data.User, @event, data.Tags);
		}
		#endregion

		#region Private Methods
		private void SaveTags(Log log, User user, Event @event, IEnumerable<Tag> tags)
		{
			foreach (var retrieved in tags.Select(GetTag))
				Dispatcher.Dispatch(AddTagEvent.CreateFrom(new TagEvent {LogId = log.Id, LogName = log.Name, EventId = @event.Id, Id = Guid.NewGuid(), Name = retrieved.Name, TagId = retrieved.Id, UserId = user.Id}));
		}

		private Tag GetTag(Tag tag)
		{
			var retrieved = TagRepository.Name(tag.Name);
			if (retrieved == null)
			{
				retrieved = new Tag();
				retrieved.Id = Guid.NewGuid();
				retrieved.Name = tag.Name;
				Dispatcher.Dispatch(AddTag.CreateFrom(retrieved));
			}
			return retrieved;
		}

		private void SaveMeasurements(User user, Log log, Event @event, IEnumerable<MeasurementData> values)
		{
			foreach (var value in values)
			{
				var measurement = GetMeasurement(value.Name, value.Unit, log, @event, user);
				Dispatcher.Dispatch(AddMeasurementValue.CreateFrom(new MeasurementValue {Id = Guid.NewGuid(), MeasurementId = measurement.Id, Quantity = value.Quantity, LogId = log.Id, UserId = user.Id}));
			}
		}

		private Measurement GetMeasurement(string name, string unit, Log log, Event @event, User user)
		{
			var measurement = MeasurementRepository.Log(log.Id).FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
			if (measurement == null)
			{
				measurement = new Measurement {Id = Guid.NewGuid(), LogId = log.Id, LogName = log.Name, EventId = @event.Id, UserId = user.Id, Name = name, Unit = unit};
				Dispatcher.Dispatch(AddMeasurement.CreateFrom(measurement));
			}
			return measurement;
		}

		private Event CreateEvent(LogData data, Log log)
		{
			var @event = new Event {Date = MergeDateAndTime(data.Date, data.Time), Id = Guid.NewGuid(), LogId = log.Id, LogName = log.Name, UserId = data.User.Id};
			Dispatcher.Dispatch(AddEvent.CreateFrom(@event));
			return @event;
		}

		private Log GetLog(LogData data)
		{
			var log = LogRepository.Name(data.Name);
			if (log == null)
			{
				log = new Log {Id = Guid.NewGuid(), Name = data.Name, UserId = data.User.Id};
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
			#endregion
		}
		#endregion
	}
}