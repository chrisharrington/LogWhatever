using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using LogWhatever.Common.Repositories;
using Event = LogWhatever.Common.Models.Page.Event;

namespace LogWhatever.Web.Controllers.Api
{
	public class EventsController : BaseApiController
	{
		#region Properties
		public IMeasurementRepository MeasurementRepository { get; set; }
		public ITagRepository TagRepository { get; set; }
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

			var measurements = MeasurementRepository.Log(log.Id).ToArray();
			var tags = TagRepository.Log(log.Id).ToArray();
			var events = EventRepository.Log(log.Id).ToArray();

			return events.OrderByDescending(x => x.Date).Select(x => new Event {
				Date = x.Date,
				Measurements = measurements.Where(y => y.EventId == x.Id),
				Tags = tags.Where(y => y.EventId == x.Id).OrderBy(y => y.Name)
			});
		}
		#endregion
	}
}