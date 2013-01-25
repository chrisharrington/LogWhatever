using System;
using System.Collections.Generic;
using System.Linq;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.Messages.Commands;

namespace LogWhatever.Repositories
{
	public class LogRepository : BaseRepository, ILogRepository
	{
		#region Public Methods
		public IEnumerable<Log> All()
		{
			return Retrieve<Log>("select * from Logs order by Name");
		}

		public Log Name(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			return All().FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
		}

		public IEnumerable<Log> User(Guid userId)
		{
			if (userId == Guid.Empty)
				throw new ArgumentNullException("userId");

			return All().Where(x => x.UserId == userId);
		}

		public void Create(Log log)
		{
			if (log == null)
				throw new ArgumentNullException("log");

			Dispatcher.Dispatch(AddLog.CreateFrom(log));
			Cache.AddToList(log);
		}
		#endregion
	}
}