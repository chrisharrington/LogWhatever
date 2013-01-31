using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using LogWhatever.Common.Extensions;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.DataService.Controllers
{
	public class ChartsController : BaseApiController
	{
		#region Properties
		public ILogRepository LogRepository { get; set; }
		public IMeasurementRepository MeasurementRepository { get; set; }
		public IEventRepository EventRepository { get; set; }
		public ITagRepository TagRepository { get; set; }
		#endregion
		
		#region Public Methods
		public dynamic Get(string logName)
		{
			var log = GetLogFromName(logName);

			dynamic result = new ExpandoObject();
			result.measurements = Measurements(log);
			result.tagRatios = TagRatios(log);
			result.eventsOverTime = EventsOverTime(log);
			result.popularDays = PopularDays(log);
			return result;
		}
		#endregion

		#region Private Methods
		private dynamic Measurements(Log log)
		{
			return MeasurementRepository.Log(log.Id).GroupBy(x => x.GroupId).Select(x => x.OrderBy(y => y.Date));
		}

		private dynamic TagRatios(Log log)
		{
			return TagRepository.Log(log.Id).GroupBy(x => x.Name).Select(x => new { x.First().Name, Count = x.Count() });
		}

		private dynamic EventsOverTime(Log log)
		{
			var events = EventRepository.Log(log.Id).OrderBy(x => x.Date);
			if (!events.Any())
				return null;

			var start = events.First().Date.BeginningOfTheWeek();
			var end = events.Last().Date.BeginningOfTheWeek();
			var results = new List<object>();
			for (var date = start; date <= end; date = date.AddDays(7))
				results.Add(new { Date = date.ToShortDateString(), Count = events.Count(x => x.Date.BeginningOfTheWeek() == date) });
			return results;
		}

		private dynamic PopularDays(Log log)
		{
			return EventRepository.Log(log.Id).GroupBy(x => x.Date.DayOfWeek).Select(x => new { Day = x.Key.ToString(), Count = x.Count() });
		}

		private Log GetLogFromName(string logName)
		{
			var log = LogRepository.Name(logName.Replace("-", " "));
			if (log == null)
				throw new ArgumentException("The log with name \"" + logName + "\" does not exist.");
			return log;
		}
		#endregion
	}
}