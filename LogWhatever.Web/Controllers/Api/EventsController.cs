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
		public IEventRepository EventRepository { get; set; }
		public IMeasurementRepository MeasurementRepository { get; set; }
		public ITagRepository TagRepository { get; set; }
		#endregion

		#region Public Methods
		[ActionName("log")]
		public IEnumerable<Event> GetForLog(string logName)
		{
			var events = EventRepository.All(x => x.LogName == logName);
			var measurements = MeasurementRepository.All(x => x.LogName == logName);
			var tags = TagRepository.All(x => x.LogName == logName);

			return events.OrderByDescending(x => x.Date).Select(x => new Event {
				Date = x.Date,
				Measurements = measurements.Where(y => y.EventId == x.Id),
				Tags = tags.Where(y => y.EventId == x.Id).OrderBy(y => y.Name)
			});
		}
		#endregion
	}
}