using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.Repositories
{
	public class EventRepository : BaseRepository, IEventRepository
	{
		#region Public Methods
		public IEnumerable<Event> Log(Guid logId)
		{
			if (logId == Guid.Empty)
				throw new ArgumentNullException("logId");

			return Query<Event>("select * from Events where LogId = @logId", new {logId});
		}
		#endregion
	}
}