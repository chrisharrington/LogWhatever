using System;
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
		#endregion
	}
}