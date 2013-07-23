using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using LogWhatever.Common.Extensions;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.Web.Controllers.Api
{
	public class MeasurementsController : BaseApiController
	{
		#region Properties
		public IMeasurementRepository MeasurementRepository { get; set; }
		public ILogRepository LogRepository { get; set; }
		#endregion

		#region Public Methods
		[ActionName("log")]
		public IEnumerable<Measurement> GetForLog([FromUri] string logName)
		{
			var measurements = MeasurementRepository.All(x => x.LogName == logName);
			return measurements.Any() ? measurements.GroupBy(x => x.Date).OrderByDescending(x => x.Key).First().OrderBy(x => x.Name).ToList() : new List<Measurement>();
		}
		#endregion
	}
}