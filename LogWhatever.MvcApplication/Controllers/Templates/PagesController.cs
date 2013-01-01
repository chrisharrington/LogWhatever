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
	public class PagesController : Controller
	{
		#region Properties
		public IMeasurementRepository MeasurementRepository { get; set; }
		public ITagRepository TagRepository { get; set; }
		public ILogRepository LogRepository { get; set; }
		public IUserRepository UserRepository { get; set; }
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
			var tags = TagRepository.User(user.Id).ToArray();
			var logs = LogRepository.User(user.Id).ToArray();
		
			//return PartialView("~/Views/Templates/Pages/Dashboard.cshtml", logs.Select(log => new DashboardLogModel {
			//	Name = log.Name,
			//	Date = log.UpdatedDate,
			//	Measurements = measurements.Where(measurement => measurement.LogId == log.Id).GroupBy(x => x.EventId).OrderByDescending(x => x.Any() ? x.First().UpdatedDate : DateTime.Now).FirstOrDefault().OrderBy(x => x.Name),
			//	Tags = tags.Where(tag => tag.LogId == log.Id).GroupBy(x => x.EventId).OrderByDescending(x => x.Any() ? x.First().UpdatedDate : DateTime.Now).FirstOrDefault().OrderBy(x => x.Name)
			//}).ToArray());
			return PartialView("~/Views/Templates/Pages/Dashboard.cshtml", new List<DashboardLogModel>());
		}
		#endregion
		
		#region Private Methods
		private User GetSignedInUser()
		{
			return UserRepository.Email(User.Identity.Name);
		}
		#endregion

		#region DashboardModel Class
		public class DashboardLogModel
		{
			#region Properties
			public DateTime Date { get; set; }
			public string Name { get; set; }
			public IEnumerable<Measurement> Measurements { get; set; }
			public IEnumerable<Tag> Tags { get; set; } 
			#endregion
		}
		#endregion
	}
}