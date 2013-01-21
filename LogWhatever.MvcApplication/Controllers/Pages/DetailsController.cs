using System.Collections.Generic;
using LogWhatever.MvcApplication.Controllers.Api;

namespace LogWhatever.MvcApplication.Controllers.Pages
{
	public class DetailsController : BaseApiController
	{
		#region Properties
		public EventsController EventsController { get; set; }
		#endregion

		#region Public Methods
		public IEnumerable<Common.Models.Page.Event> Get(string name)
		{
			return EventsController.GetForLog(name);
		}
		#endregion
	}
}