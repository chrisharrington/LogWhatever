using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace LogWhatever.MvcApplication
{
	public class MvcApplication : System.Web.HttpApplication
	{
		#region Event Handlers
		protected void Application_Start()
		{
			RegisterRoutes();

			GlobalFilters.Filters.Add(new HandleErrorAttribute());
		}
		#endregion

		#region Private Methods
		private static void RegisterRoutes()
		{
			var routes = RouteTable.Routes;

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Shared", action = "Index", id = UrlParameter.Optional });
			routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
		}
		#endregion
	}
}