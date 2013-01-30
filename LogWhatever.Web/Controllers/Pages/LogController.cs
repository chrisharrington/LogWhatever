using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.MvcApplication.Controllers.Api;

namespace LogWhatever.MvcApplication.Controllers.Pages
{
	[Authorize]
	public class LogController : BaseApiController
	{
		#region Properties
		public ILogRepository LogRepository { get; set; }
		public IMeasurementRepository MeasurementRepository { get; set; }
		public ITagRepository TagRepository { get; set; }
		public ITagEventRepository TagEventRepository { get; set; }
		#endregion

		#region Public Methods
		[ActionName("measurements")]
		public PartialViewResult Measurements(string name)
		{
			var log = LogRepository.Name(name);
			var measurements = log == null ? new List<Measurement>() : MeasurementRepository.Log(log.Id);
			if (measurements.Any())
				measurements = measurements.GroupBy(x => x.EventId).First();
			return PartialView("~/Views/Templates/Log/Measurements.cshtml", measurements.ToList().OrderBy(x => x.Name));
		}

		[ActionName("tags")]
		public PartialViewResult Tags(string name)
		{
			return PartialView("~/Views/Templates/Log/Tags.cshtml", TagEventRepository.LatestForUserAndLog(GetCurrentlySignedInUser().Id, name));
		}
		#endregion
	}
}