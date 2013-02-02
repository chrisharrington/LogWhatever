using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.DataService.Controllers
{
	public class MeasurementsController : BaseApiController
	{
		#region Properties
		public ILogRepository LogRepository { get; set; }
		public IMeasurementRepository MeasurementRepository { get; set; }
		public IEventRepository EventRepository { get; set; }
		#endregion

		#region Public Methods
		public IEnumerable<Measurement> Get()
		{
			return MeasurementRepository.All();

			//var log = LogRepository.Name(logName);
			//if (log == null)
			//	return new List<Measurement>();

			//var logMeasurements = MeasurementRepository.Log(log.Id);
			//return logMeasurements.Any() ? logMeasurements.GroupBy(x => x.Date).OrderByDescending(x => x.Key).First().OrderBy(x => x.Name).ToList() : new List<Measurement>();
		}

		public void Post(Measurement measurement)
		{
			MeasurementRepository.Create(measurement);
		}
		#endregion
	}
}