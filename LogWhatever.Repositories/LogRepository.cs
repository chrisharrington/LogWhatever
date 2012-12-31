using System;
using System.Collections.Generic;
using System.Linq;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.Repositories
{
	public class LogRepository : BaseRepository, ILogRepository
	{
		#region Public Methods
		public Log Name(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			return Query<Log>("select * from Logs where Name = @name", new {name}).FirstOrDefault();
		}

		public IEnumerable<Log> User(Guid userId)
		{
			if (userId == Guid.Empty)
				throw new ArgumentNullException("userId");

			return Query<Log>("select * from Logs where UserId = @userId", new {userId});
		}
		#endregion
	}
}