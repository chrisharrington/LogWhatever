using System;
using System.Collections.Generic;
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
		public IEventRepository EventRepository { get; set; }
		public ITagRepository TagRepository { get; set; }
		#endregion

		#region Public Methods
		[ActionName("measurements")]
		[AcceptVerbs("GET")]
		public dynamic GetMeasurements([FromUri] string logName)
		{
			return new List<Measurement>();
		}

		[ActionName("tag-ratios")]
		[AcceptVerbs("GET")]
		public dynamic GetTagRatios([FromUri] string logName)
		{
			return TagRepository.Log(GetLogFromName(logName).Id).GroupBy(x => x.Name).Select(x => new {x.First().Name, Count = x.Count()});
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