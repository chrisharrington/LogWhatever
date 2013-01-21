using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using LogWhatever.Common.Repositories;
using Event = LogWhatever.Common.Models.Page.Event;

namespace LogWhatever.MvcApplication.Controllers.Api
{
	public class EventsController : BaseApiController
	{
		#region Properties
		public IMeasurementRepository MeasurementRepository { get; set; }
		public IMeasurementValueRepository MeasurementValueRepository { get; set; }
		public ITagEventRepository TagEventRepository { get; set; }
		public ILogRepository LogRepository { get; set; }
		public IEventRepository EventRepository { get; set; }
		#endregion

		#region Public Methods
		[ActionName("log")]
		public IEnumerable<Event> GetForLog(string logName)
		{
			if (string.IsNullOrEmpty(logName))
				throw new HttpResponseException(HttpStatusCode.NotFound);

			var log = LogRepository.Name(logName);
			if (log == null)
				return new List<Event>();

			var measurements = MeasurementRepository.Log(log.Id).ToDictionary(x => x.Id);
			var measurementValues = MeasurementValueRepository.Log(log.Id).ToArray();
			var tags = TagEventRepository.Log(log.Id).ToArray();
			var events = EventRepository.Log(log.Id).ToArray();

			return events.OrderByDescending(x => x.Date).Select(x => new Event {
				Date = x.Date,
				Measurements = measurementValues.Where(y => y.EventId == x.Id).Select(y => new Common.Models.Page.Measurement { Name = measurements[y.MeasurementId].Name, Quantity = y.Quantity, Unit = measurements[y.MeasurementId].Unit }).OrderBy(y => y.Name),
				Tags = tags.Where(y => y.EventId == x.Id).OrderBy(y => y.Name)
			});
		}
		#endregion
	}
}