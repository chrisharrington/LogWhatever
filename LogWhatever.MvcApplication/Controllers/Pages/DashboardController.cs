using System.Collections.Generic;
using System.Linq;
using LogWhatever.Common.Extensions;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.MvcApplication.Controllers.Api;

namespace LogWhatever.MvcApplication.Controllers.Pages
{
	public class DashboardController : BaseApiController
	{
		#region Properties
		public IMeasurementRepository MeasurementRepository { get; set; }
		public IMeasurementValueRepository MeasurementValueRepository { get; set; }
		public ITagEventRepository TagEventRepository { get; set; }
		public ILogRepository LogRepository { get; set; }
		public IEventRepository EventRepository { get; set; }
		#endregion

		#region Public Methods
		public IEnumerable<IEnumerable<Common.Models.Page.Log>> Get()
		{
			const int columns = 5;
			var user = GetCurrentlySignedInUser();
			var measurements = MeasurementRepository.User(user.Id).ToArray();
			var measurementValues = MeasurementValueRepository.User(user.Id);
			var logs = LogRepository.User(user.Id).ToArray();
			var events = EventRepository.Latest(user.Id).ToDictionary(x => x.LogId);

			var models = logs.Select(log => new Common.Models.Page.Log {
				Name = log.Name,
				Date = events[log.Id].Date,
				Measurements = GetMeasurementValues(measurements, log, measurementValues),
				Tags = TagEventRepository.LatestForUserAndLog(user.Id, log.Name)
			}).OrderByDescending(x => x.Date).ToArray();

			var list = new List<IEnumerable<Common.Models.Page.Log>>();
			for (var i = 0; i < columns; i++)
				list.Add(models.Skip(i).TakeEvery(columns));
			return list;
		}
		#endregion

		#region Private Methods
		private IEnumerable<Common.Models.Page.Measurement> GetMeasurementValues(IEnumerable<Measurement> measurements, Log log, IEnumerable<MeasurementValue> measurementValues)
		{
			return measurements.Where(x => x.LogId == log.Id).Select(x => new Common.Models.Page.Measurement { Name = x.Name, Quantity = GetLatestMeasurementValueQuantity(x, measurementValues), Unit = x.Unit });
		}

		private decimal GetLatestMeasurementValueQuantity(Measurement measurement, IEnumerable<MeasurementValue> values)
		{
			if (values.All(x => x.MeasurementId != measurement.Id))
				return 0;

			return values.Where(x => x.MeasurementId == measurement.Id).OrderByDescending(x => x.UpdatedDate).First().Quantity;
		}
		#endregion
	}
}