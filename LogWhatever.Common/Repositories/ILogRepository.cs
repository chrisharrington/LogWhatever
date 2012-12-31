using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Repositories
{
	public interface ILogRepository
	{
		#region Public Methods
		Log Name(string name);
		IEnumerable<Log> User(Guid userId);
		#endregion
	}
}