using LogWhatever.Common.Models;
using LogWhatever.Repositories;

namespace LogWhatever.RemoteRepositories
{
	public class RemoteLogRepository : LogRepository
	{
		#region Properties
		public BaseRemoteRepository Repository { get; set; }
		#endregion

		#region Public Methods
		public override System.Collections.Generic.IEnumerable<Log> All(System.Func<Log, bool> filter = null)
		{
			return Repository.All("logs", filter);
		}

		public override void Create(Log log)
		{
			Repository.HttpRequestor.Post(ConfigurationProvider.DataServiceLocation + "logs", log, Repository.SessionManager.GetCurrent());
			Cache.AddToList(log);
		}
		#endregion
	}
}