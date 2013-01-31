using LogWhatever.Common.Models;
using Log = LogWhatever.Common.Models.Data.Log;

namespace LogWhatever.Common.Service.Http
{
	public interface ILogsDataService
	{
		void Save(Session session, Log data);
	}
}