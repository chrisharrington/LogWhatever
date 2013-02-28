using System;
using System.Collections.Generic;
using System.Linq;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.DataService.Controllers
{
	public class LogsController : BaseApiController
	{
		#region Properties
		public ILogRepository LogRepository { get; set; }
		#endregion

		#region Public Methods
		public IEnumerable<Log> Get(Guid? userId = null)
		{
			return LogRepository.All(x => userId == null || x.UserId == userId.Value).OrderBy(x => x.Name);
		} 

		public void Post(Log log)
		{
			LogRepository.Create(log);
		}
		#endregion
	}
}