using System.Collections.Generic;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.DataService.Controllers
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
		public IEnumerable<Event> Get()
		{
			return EventRepository.All();
		}
		#endregion
	}
}