using System;
using System.Collections.Generic;
using System.Linq;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.Messages.Commands;

namespace LogWhatever.Repositories
{
	public class SessionRepository : BaseRepository, ISessionRepository
	{
		#region Public Methods
		public IEnumerable<Session> All(Func<Session, bool> filter = null)
		{
			return Retrieve("select * from Sessions", filter);
		}

		public void Create(Session session)
		{
			if (session == null)
				throw new ArgumentNullException("session");

			Cache.AddToList(session);
			Dispatcher.Dispatch(AddSession.CreateFrom(session));
		}

		public void Delete(Guid id)
		{
			if (id == Guid.Empty)
				throw new ArgumentNullException("id");

			Cache.RemoveFromList<Session>(id);
			Dispatcher.Dispatch(new DeleteSession {Id = id});
		}

		public Session Id(Guid id)
		{
			if (id == Guid.Empty)
				throw new ArgumentNullException("id");

			return All(x => x.Id == id).FirstOrDefault();
		}
		#endregion
	}
}