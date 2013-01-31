using System.Collections.Generic;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Service.Http
{
	public interface ITagsDataService
	{
		#region Public Methods
		IEnumerable<Tag> Log(Session session, string logName);
		#endregion
	}
}