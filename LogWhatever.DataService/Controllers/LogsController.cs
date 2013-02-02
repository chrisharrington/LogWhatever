using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
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
		public IEnumerable<Log> Get()
		{
			return LogRepository.All().OrderBy(x => x.Name);
		} 

		public void Post(Log log)
		{
			LogRepository.Create(log);
		}
		#endregion
	}
}