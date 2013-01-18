using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.Service.Authentication;

namespace LogWhatever.MvcApplication.Controllers.Templates
{
	[CustomAuthorize]
	public class PagesController : BaseController
	{
		#region Properties
		public IMeasurementRepository MeasurementRepository { get; set; }
		public IMeasurementValueRepository MeasurementValueRepository { get; set; }
		public ITagEventRepository TagEventRepository { get; set; }
		public ILogRepository LogRepository { get; set; }
		public IEventRepository EventRepository { get; set; }
		#endregion

		#region Public Methods
		[Service.Authentication.AllowAnonymous]
		[ActionName("welcome")]
		public PartialViewResult Welcome()
		{
			return PartialView("~/Views/Templates/Pages/Welcome.cshtml");
		}

		[ActionName("dashboard")]
		public PartialViewResult Dashboard()
		{
			var user = GetSignedInUser();
			var measurements = MeasurementRepository.User(user.Id).ToArray();
			var measurementValues = MeasurementValueRepository.User(user.Id);
			var logs = LogRepository.User(user.Id).ToArray();

			return PartialView("~/Views/Templates/Pages/Dashboard.cshtml", logs.Select(log => new LogModel {
				Name = log.Name,
				Date = log.UpdatedDate,
				Measurements = GetMeasurementValues(measurements, log, measurementValues),
				Tags = TagEventRepository.LatestForUserAndLog(user.Id, log.Name)
			}).OrderByDescending(x => x.Date).ToArray());
		}

		[ActionName("details")]
		public ActionResult Details(string name)
		{
			Log log;
			if (string.IsNullOrEmpty(name) || (log = LogRepository.Name(name)) == null)
				return HttpNotFound();

			var measurements = MeasurementRepository.Log(log.Id).ToDictionary(x => x.Id);
			var measurementValues = MeasurementValueRepository.Log(log.Id).ToArray();
			var tags = TagEventRepository.Log(log.Id).ToArray();
			var events = EventRepository.Log(log.Id).ToArray();

			return PartialView("~/Views/Templates/Pages/Details.cshtml", events.OrderByDescending(x => x.Date).Select(x => new EventModel {
				Date = x.Date,
                Measurements = measurementValues.Where(y => y.EventId == x.Id).Select(y => new MeasurementModel { Name = measurements[y.MeasurementId].Name, Quantity = y.Quantity, Unit = measurements[y.MeasurementId].Unit }).OrderBy(y => y.Name),
				Tags = tags.Where(y => y.EventId == x.Id).OrderBy(y => y.Name)
			}));
		}
		#endregion
		
		#region Private Methods
		private IEnumerable<MeasurementModel> GetMeasurementValues(IEnumerable<Measurement> measurements, Log log, IEnumerable<MeasurementValue> measurementValues)
		{
			return measurements.Where(x => x.LogId == log.Id).Select(x => new MeasurementModel { Name = x.Name, Quantity = GetLatestMeasurementValueQuantity(x, measurementValues), Unit = x.Unit });
		}

		private User GetSignedInUser()
		{
			return UserRepository.Email(User.Identity.Name);
		}

		private decimal GetLatestMeasurementValueQuantity(Measurement measurement, IEnumerable<MeasurementValue> values)
		{
			if (values.All(x => x.MeasurementId != measurement.Id))
				return 0;

			return values.Where(x => x.MeasurementId == measurement.Id).OrderByDescending(x => x.UpdatedDate).First().Quantity;
		}
		#endregion

		#region DashboardModel Class
		public class LogModel
		{
			#region Properties
			public DateTime Date { get; set; }
			public string Name { get; set; }
			public IEnumerable<MeasurementModel> Measurements { get; set; }
			public IEnumerable<TagEvent> Tags { get; set; } 
			#endregion
		}

		public class EventModel
		{
			#region Properties
			public DateTime Date { get; set; }
			public IEnumerable<MeasurementModel> Measurements { get; set; }
			public IEnumerable<TagEvent> Tags { get; set; } 
			#endregion
		}

		public class MeasurementModel
		{
			#region Properties
			public string Name { get; set; }
			public decimal Quantity { get; set; }
			public string Unit { get; set; }
			#endregion
		}
		#endregion
	}
}