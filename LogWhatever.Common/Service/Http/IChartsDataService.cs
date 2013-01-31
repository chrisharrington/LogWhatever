using LogWhatever.Common.Models;

namespace LogWhatever.Common.Service.Http
{
	public interface IChartsDataService
	{
		#region Public Methods
		dynamic Measurements(Session session, string logName);
		dynamic TagRatios(Session session, string logName);
		dynamic EventsOverTime(Session session, string logName);
		dynamic PopularDays(Session session, string logName);
		dynamic All(Session session, string logName);
		#endregion
	}
}