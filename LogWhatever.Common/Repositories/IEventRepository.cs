using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Repositories
{
	public interface IEventRepository : IRepository<Event>
	{
		#region Public Methods
		IEnumerable<Event> Log(Guid logId);
		IEnumerable<Event> Latest(Guid userId);
		void Create(Event @event);
		#endregion
	}
}