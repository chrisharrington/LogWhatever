using System.Collections.Generic;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Service.Http
{
	public interface IEventsDataService
	{
		#region Public Methods
		IEnumerable<Models.Page.Event> Log(Session session, string logName);
		#endregion
	}
}