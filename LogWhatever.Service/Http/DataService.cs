using LogWhatever.Common.Service.Http;

namespace LogWhatever.Service.Http
{
	public class DataService : IDataService
	{
		#region Properties
		public ISessionDataService Session { get; set; }
		public IChartsDataService Chart { get; set; }
		public IEventsDataService Events { get; set; }
		public ILogsDataService Logs { get; set; }
		public IMeasurementsDataService Measurements { get; set; }
		public ITagsDataService Tags { get; set; }
		public ISessionDataService Users { get; set; }
		#endregion
	}
}