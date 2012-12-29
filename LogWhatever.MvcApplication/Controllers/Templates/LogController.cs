using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

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
			return PartialView("~/Views/Templates/Log/Measurements.cshtml", (log == null ? new List<Measurement>() : MeasurementRepository.LogId(log.Id)).OrderBy(x => x.Name));
		}

		[ActionName("tags")]
		public PartialViewResult Tags(string name)
		{
			var log = LogRepository.Name(name);
			return PartialView("~/Views/Templates/Log/Tags.cshtml", (log == null ? new List<Tag>() : TagRepository.LogId(log.Id)).OrderBy(x => x.Name));
		}
		#endregion
	}
}