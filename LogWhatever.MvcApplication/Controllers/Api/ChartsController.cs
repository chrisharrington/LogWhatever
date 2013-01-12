using System;
using System.Dynamic;
using System.Linq;
using System.Web.Http;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.MvcApplication.Controllers.Api
{
	public class ChartsController : BaseApiController
	{
		#region Properties
		public ILogRepository LogRepository { get; set; }
		public IMeasurementRepository MeasurementRepository { get; set; }
		public IMeasurementValueRepository MeasurementValueRepository { get; set; }
		#endregion

		#region Public Methods
		[ActionName("measurements")]
		[AcceptVerbs("GET")]
		public dynamic GetMeasurements([FromUri] string logName)
		{
			var log = GetLogFromName(logName);
			var values = MeasurementValueRepository.Log(log.Id).ToArray();

			dynamic result = new ExpandoObject();
			foreach (var measurement in MeasurementRepository.Log(log.Id))
			{
				result.Name = measurement.Name;
				result.Data = values.Where(x => x.MeasurementId == measurement.Id).Select(x => new {x.Quantity, x.UpdatedDate});
			}
			return result;
		}
		#endregion

		#region Private Methods
		private Log GetLogFromName(string logName)
		{
			var log = LogRepository.Name(logName);
			if (log == null)
				throw new ArgumentException("The log with name \"" + logName + "\" does not exist.");
			return log;
		}
		#endregion
	}
}