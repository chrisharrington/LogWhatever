using System.Collections.Generic;
using LogWhatever.Web.Controllers.Api;

namespace LogWhatever.Web.Controllers.Pages
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