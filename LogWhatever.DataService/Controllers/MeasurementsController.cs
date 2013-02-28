using System;
using System.Collections.Generic;
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
		public IEnumerable<Measurement> Get(Guid? userId = null)
		{
			return MeasurementRepository.All(x => userId == null || x.UserId == userId.Value);
		}

		public void Post(Measurement measurement)
		{
			MeasurementRepository.Create(measurement);
		}
		#endregion
	}
}