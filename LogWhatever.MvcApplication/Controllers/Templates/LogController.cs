using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.Common.Extensions;

namespace LogWhatever.MvcApplication.Controllers.Templates
{
	[Authorize]
	public class LogController : Controller
	{
		#region Properties
		public ILogRepository LogRepository { get; set; }
		public IMeasurementRepository MeasurementRepository { get; set; }
		public ITagRepository TagRepository { get; set; }
		#endregion

		#region Public Methods
		[ActionName("measurements")]
		public PartialViewResult Measurements(string name)
		{
			var log = LogRepository.Name(name);
			var measurements = log == null ? new List<Measurement>() : MeasurementRepository.LogId(log.Id);
			if (measurements.Any())
				measurements = measurements.GroupBy(x => x.EventId).First();
			return PartialView("~/Views/Templates/Log/Measurements.cshtml", measurements.ToList().OrderBy(x => x.Name));
		}

		[ActionName("tags")]
		public PartialViewResult Tags(string name)
		{
			var log = LogRepository.Name(name);
			return PartialView("~/Views/Templates/Log/Tags.cshtml", (log == null ? new List<Tag>() : TagRepository.LogId(log.Id)).Distinct((x, y) => x.Name == y.Name).OrderBy(x => x.Name));
		}
		#endregion
	}
}