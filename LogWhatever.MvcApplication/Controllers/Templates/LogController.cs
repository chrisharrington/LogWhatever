using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.MvcApplication.Controllers.Templates
{
	[System.Web.Mvc.Authorize]
	public class LogController : Controller
	{
		#region Properties
		public ILogRepository LogRepository { get; set; }
		public IMeasurementRepository MeasurementRepository { get; set; }
		#endregion

		#region Public Methods
		[System.Web.Mvc.ActionName("measurements")]
		public PartialViewResult Measurements(string name)
		{
			return PartialView("~/Views/Templates/Log/Measurements.cshtml", MeasurementRepository.LogId(GetLogFromName(name).Id).OrderBy(x => x.Name));
		}
		#endregion

		#region Private Methods
		private Log GetLogFromName(string name)
		{
			var log = LogRepository.Name(name);
			if (log == null)
				throw new ArgumentException("The name \"" + name + "\" corresponds to no log.");
			return log;
		}
		#endregion
	}
}