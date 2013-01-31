using System.Collections.Generic;
using System.Linq;
using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Http;

namespace LogWhatever.Service.Http
{
	public class EventsDataService : BaseDataService, IEventsDataService
	{
		#region Properties
		public override string SubLocation
		{
			get { return "events/"; }
		}
		#endregion

		#region Public Methods
		public IEnumerable<Common.Models.Page.Event> Log(Session session, string logName)
		{
			var events = All<Event>(x => x.LogName == logName);
			var measurements = All<Measurement>(x => x.LogName == logName);
			var tags = All<Tag>(x => x.LogName == logName);

			return events.OrderByDescending(x => x.Date).Select(x => new Common.Models.Page.Event {
				Date = x.Date,
				Measurements = measurements.Where(y => y.EventId == x.Id),
				Tags = tags.Where(y => y.EventId == x.Id).OrderBy(y => y.Name)
			});
		}
		#endregion
	}
}