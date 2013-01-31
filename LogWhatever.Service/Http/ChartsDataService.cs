using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Http;

namespace LogWhatever.Service.Http
{
	public class ChartsDataService : BaseDataService, IChartsDataService
	{
		#region Properties
		public override string SubLocation
		{
			get { return "charts/"; }
		}
		#endregion

		#region Public Methods
		public dynamic Measurements(Session session, string logName)
		{
			return HttpRequestor.Get<object>(DataServiceLocation + "measurements", new {logName, auth = session.Id.ToString()});
		}

		public dynamic TagRatios(Session session, string logName)
		{
			return HttpRequestor.Get<object>(DataServiceLocation + "tag-ratios", new {logName, auth = session.Id.ToString()});
		}

		public dynamic EventsOverTime(Session session, string logName)
		{
			return HttpRequestor.Get<object>(DataServiceLocation + "events-over-time", new {logName, auth = session.Id.ToString()});
		}

		public dynamic PopularDays(Session session, string logName)
		{
			return HttpRequestor.Get<object>(DataServiceLocation + "popular-days", new {logName, auth = session.Id.ToString()});
		}

		public dynamic All(Session session, string logName)
		{
			return HttpRequestor.Get<object>(DataServiceLocation + "", new {logName, auth = session.Id.ToString()});
		}
		#endregion
	}
}