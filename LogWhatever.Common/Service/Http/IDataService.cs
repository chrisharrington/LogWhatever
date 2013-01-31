namespace LogWhatever.Common.Service.Http
{
	public interface IDataService
	{
		#region Properties
		ISessionDataService Session { get; set; }
		IChartsDataService Chart { get; set; }
		IEventsDataService Events { get; set; }
		ILogsDataService Logs { get; set; }
		IMeasurementsDataService Measurements { get; set; }
		ITagsDataService Tags { get; set; }
		ISessionDataService Users { get; set; }
		#endregion
	}
}