using System;
using System.Collections.Generic;
using System.Linq;
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

		public IEnumerable<Event> Latest(Guid userId)
		{
			if (userId == Guid.Empty)
				throw new ArgumentNullException("userId");

			return Query<Event>("select * from Events where UserId = @userId", new {userId})
				.GroupBy(x => x.LogId)
				.Select(y => y.OrderByDescending(z => z.Date).First());
		}
		#endregion
	}
}