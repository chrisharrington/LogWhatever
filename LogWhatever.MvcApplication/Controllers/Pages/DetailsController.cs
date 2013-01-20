using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.MvcApplication.Controllers.Api;

namespace LogWhatever.MvcApplication.Controllers.Pages
{
	public class DetailsController : BaseApiController
	{
		#region Properties
		public IMeasurementRepository MeasurementRepository { get; set; }
		public IMeasurementValueRepository MeasurementValueRepository { get; set; }
		public ITagEventRepository TagEventRepository { get; set; }
		public ILogRepository LogRepository { get; set; }
		public IEventRepository EventRepository { get; set; }
		#endregion

		#region Public Methods
		public IEnumerable<EventModel> Get(string name)
		{
			Log log;
			if (string.IsNullOrEmpty(name) || (log = LogRepository.Name(name.Replace("-", " "))) == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			var measurements = MeasurementRepository.Log(log.Id).ToDictionary(x => x.Id);
			var measurementValues = MeasurementValueRepository.Log(log.Id).ToArray();
			var tags = TagEventRepository.Log(log.Id).ToArray();
			var events = EventRepository.Log(log.Id).ToArray();

			return events.OrderByDescending(x => x.Date).Select(x => new EventModel
			{
				Date = x.Date,
				Measurements = measurementValues.Where(y => y.EventId == x.Id).Select(y => new MeasurementModel { Name = measurements[y.MeasurementId].Name, Quantity = y.Quantity, Unit = measurements[y.MeasurementId].Unit }).OrderBy(y => y.Name),
				Tags = tags.Where(y => y.EventId == x.Id).OrderBy(y => y.Name)
			});
		}
		#endregion

		public class MeasurementModel
		{
			#region Properties
			public string Name { get; set; }
			public decimal Quantity { get; set; }
			public string Unit { get; set; }
			#endregion
		}

		public class EventModel
		{
			#region Properties
			public DateTime Date { get; set; }
			public IEnumerable<MeasurementModel> Measurements { get; set; }
			public IEnumerable<TagEvent> Tags { get; set; }
			#endregion
		}
	}
}