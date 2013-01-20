﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Http;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.MvcApplication.Controllers.Api
{
	public class ChartsController : BaseApiController
	{
		#region Properties
		public ILogRepository LogRepository { get; set; }
		public IMeasurementRepository MeasurementRepository { get; set; }
		public IMeasurementValueRepository MeasurementValueRepository { get; set; }
		public ITagEventRepository TagEventRepository { get; set; }
		public IEventRepository EventRepository { get; set; }
		#endregion

		#region Public Methods
		[ActionName("measurements")]
		[AcceptVerbs("GET")]
		public dynamic GetMeasurements([FromUri] string logName)
		{
			var log = GetLogFromName(logName);
			var values = MeasurementValueRepository.Log(log.Id).ToArray();
			var events = EventRepository.Log(log.Id).ToDictionary(x => x.Id);

			var result = new List<object>();
			foreach (var measurement in MeasurementRepository.Log(log.Id))
				result.Add(new {measurement.Name, Data = values.Where(x => x.MeasurementId == measurement.Id).OrderByDescending(x => events[x.EventId].Date).Select(x => new {x.Quantity, events[x.EventId].Date}) });
			return result;
		}

		[ActionName("tag-ratios")]
		[AcceptVerbs("GET")]
		public dynamic GetTagRatios([FromUri] string logName)
		{
			return TagEventRepository.Log(GetLogFromName(logName).Id).GroupBy(x => x.Name).Select(x => new {x.First().Name, Count = x.Count()});
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