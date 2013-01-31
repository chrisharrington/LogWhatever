using System;
using LogWhatever.Common.Repositories;

namespace LogWhatever.Web.Controllers.Api
{
	public class LogsController : AuthorizedController
	{
		#region Properties
		public ILogRepository LogRepository { get; set; }
		#endregion

		#region Public Methods
		public void Post(Common.Models.Data.Log data)
		{
			//LogRepository.Create(data);
			throw new NotImplementedException();
		}
		#endregion
	}
}