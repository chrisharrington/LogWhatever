using System.Web.Mvc;

namespace LogWhatever.Web.Controllers
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