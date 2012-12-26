using System.Web.Mvc;

namespace LogWhatever.MvcApplication.Controllers
{
	public class TemplatesController : Controller
	{
		#region Public Methods
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