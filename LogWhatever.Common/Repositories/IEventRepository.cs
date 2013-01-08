using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Repositories
{
	public interface IEventRepository
	{
		#region Public Methods
		IEnumerable<Event> Log(Guid logId);
		#endregion
	}
}