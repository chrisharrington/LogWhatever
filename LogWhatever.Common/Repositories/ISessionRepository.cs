using System;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Repositories
{
	public interface ISessionRepository : IRepository<Session>
	{
		#region Public Methods
		void Create(Session session);
		void Delete(Guid id);
		Session Id(Guid id);
		#endregion
	}
}