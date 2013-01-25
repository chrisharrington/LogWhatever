using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Repositories
{
	public interface ILogRepository : IRepository<Log>
	{
		#region Public Methods
		Log Name(string name);
		IEnumerable<Log> User(Guid userId);
		void Create(Log log);
		#endregion
	}
}