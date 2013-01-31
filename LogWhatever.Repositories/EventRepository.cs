using System;
using System.Collections.Generic;
using System.Linq;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.Messages.Commands;

namespace LogWhatever.Repositories
{
	public class EventRepository : BaseRepository, IEventRepository
	{
		#region Public Methods
		public virtual IEnumerable<Event> All(Func<Event, bool> filter = null)
		{
			return Retrieve("select * from Events", filter);
		}

		public IEnumerable<Event> Log(Guid logId)
		{
			if (logId == Guid.Empty)
				throw new ArgumentNullException("logId");

			return All().Where(x => x.LogId == logId);
		}

		public IEnumerable<Event> Latest(Guid userId)
		{
			if (userId == Guid.Empty)
				throw new ArgumentNullException("userId");

			return All().Where(x => x.UserId == userId).GroupBy(x => x.LogId).Select(x => x.OrderByDescending(y => y.Date).First());
		}

		public void Create(Event @event)
		{
			if (@event == null)
				throw new ArgumentNullException("@event");

			Dispatcher.Dispatch(AddEvent.CreateFrom(@event));
			Cache.AddToList(@event);
		}
		#endregion
	}
}