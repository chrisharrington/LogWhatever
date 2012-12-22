using System.Web.Mvc;

namespace LogWhatever.MvcApplication.Controllers
{
	public class SharedController : Controller
	{
		#region Public Methods
		public ViewResult Index()
		{
			return View("~/Views/Shared/Layout.cshtml");
		}
		#endregion
	}
}