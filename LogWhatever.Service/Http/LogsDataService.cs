using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Http;
using Log = LogWhatever.Common.Models.Data.Log;

namespace LogWhatever.Service.Http
{
	public class LogsDataService : BaseDataService, ILogsDataService
	{
		#region Properties
		public override string SubLocation
		{
			get { return "logs/"; }
		}
		#endregion

		#region Public Methods
		public void Save(Session session, Log data)
		{
			HttpRequestor.Post(DataServiceLocation, data);
		}
		#endregion
	}
}