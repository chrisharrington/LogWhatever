using System.Collections.Generic;
using System.Linq;
using LogWhatever.Common.Extensions;
using LogWhatever.Common.Repositories;
using LogWhatever.Web.Controllers.Api;

namespace LogWhatever.Web.Controllers.Pages
{
	public class DashboardController : BaseApiController
	{
		#region Properties
		public IMeasurementRepository MeasurementRepository { get; set; }
		public ITagRepository TagRepository { get; set; }
		public IEventRepository EventRepository { get; set; }
		public ILogRepository LogRepository { get; set; }
		#endregion

		#region Public Methods
		public IEnumerable<IEnumerable<Common.Models.Page.Log>> Get()
		{
			const int columns = 5;

			var user = GetCurrentSession().User;
			var measurements = MeasurementRepository.All(x => x.UserId == user.Id).ToArray();
			var logs = LogRepository.All(x => x.UserId == user.Id).ToArray();
			var events = EventRepository.Latest(user.Id).ToDictionary(x => x.LogId);

			var models = logs.Select(log => new Common.Models.Page.Log {
				Name = log.Name,
				Date = events[log.Id].Date,
				Measurements = measurements.Where(x => x.LogId == log.Id).GroupBy(x => x.Name).Select(x => x.OrderByDescending(y => y.UpdatedDate).First()),
				Tags = TagRepository.LatestForUserAndLog(user.Id, log.Name)
			}).OrderByDescending(x => x.Date).ToArray();

			var list = new List<IEnumerable<Common.Models.Page.Log>>();
			for (var i = 0; i < columns; i++)
				list.Add(models.Skip(i).TakeEvery(columns));
			return list;
		}
		#endregion
	}
}