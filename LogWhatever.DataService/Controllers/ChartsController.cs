using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
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
		[ActionName("measurements")]
		[AcceptVerbs("GET")]
		public dynamic GetMeasurements([FromUri] string logName)
		{
			return MeasurementRepository.Log(GetLogFromName(logName).Id).GroupBy(x => x.GroupId).Select(x => x.OrderBy(y => y.Date));
		}

		[ActionName("tag-ratios")]
		[AcceptVerbs("GET")]
		public dynamic GetTagRatios([FromUri] string logName)
		{
			return TagRepository.Log(GetLogFromName(logName).Id).GroupBy(x => x.Name).Select(x => new {x.First().Name, Count = x.Count()});
		}

		[ActionName("events-per-week")]
		[AcceptVerbs("GET")]
		public dynamic GetEventsPerWeek([FromUri] string logName)
		{
			var events = EventRepository.Log(GetLogFromName(logName).Id).OrderBy(x => x.Date);
			if (!events.Any())
				return null;

			var start = events.First().Date.BeginningOfTheWeek();
			var end = events.Last().Date.BeginningOfTheWeek();
			var results = new List<object>();
			for (var date = start; date <= end; date = date.AddDays(7))
				results.Add(new { Date = date.ToShortDateString(), Count = events.Count(x => x.Date.BeginningOfTheWeek() == date) });
			return results;
		}

		[ActionName("popular-days")]
		[AcceptVerbs("GET")]
		public dynamic GetPopularDays([FromUri] string logName)
		{
			var log = GetLogFromName(logName);
			return EventRepository.Log(log.Id).GroupBy(x => x.Date.DayOfWeek).Select(x => new {Day = x.Key.ToString(), Count = x.Count()});
		}
		#endregion

		#region Private Methods
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