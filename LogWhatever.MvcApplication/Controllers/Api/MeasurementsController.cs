using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.MvcApplication.Controllers.Api
{
	public class MeasurementsController : BaseApiController
	{
		#region Properties
		public ILogRepository LogRepository { get; set; }
		public IMeasurementRepository MeasurementRepository { get; set; }
		public IMeasurementValueRepository MeasurementValueRepository { get; set; }
		#endregion

		#region Public Methods
		[ActionName("log")]
		public IEnumerable<Common.Models.Page.Measurement> GetForLog([FromUri] string logName)
		{
			var log = LogRepository.Name(logName);
			if (log == null)
				return new List<Common.Models.Page.Measurement>();

			var values = MeasurementValueRepository.Log(log.Id).GroupBy(x => x.MeasurementId);
			var measurements = MeasurementRepository.Log(log.Id);
			if (measurements.Any())
				measurements = measurements.GroupBy(x => x.EventId).First();
			return measurements.OrderBy(x => x.Name).Select(x => new Common.Models.Page.Measurement {Name = x.Name, Quantity = GetQuantity(x, values), Unit = x.Unit});
		}
		#endregion

		#region Private Methods
		private decimal GetQuantity(Measurement measurement, IEnumerable<IGrouping<Guid, MeasurementValue>> values)
		{
			if (values.All(x => x.First().MeasurementId != measurement.Id))
				return 0;

			return values.First(x => x.First().MeasurementId == measurement.Id).OrderByDescending(x => x.UpdatedDate).First().Quantity;
		}
		#endregion
	}
}