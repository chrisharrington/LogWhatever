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
		#endregion
	}
}