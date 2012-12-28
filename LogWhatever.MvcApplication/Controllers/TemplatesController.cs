﻿using System.Web.Mvc;
using LogWhatever.Service.Authentication;

namespace LogWhatever.MvcApplication.Controllers
{
	[CustomAuthorize]
	public class TemplatesController : Controller
	{
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
			return PartialView("~/Views/Templates/Pages/Dashboard.cshtml");
		}
		#endregion
	}
}