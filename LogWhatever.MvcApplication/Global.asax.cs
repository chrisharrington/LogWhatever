using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using LogWhatever.Common.Adapters.Bundling;
using LogWhatever.Container;
using log4net;

namespace LogWhatever.MvcApplication
{
	public class MvcApplication : HttpApplication
	{
		#region Data Members
		internal IContainer _container;
		#endregion

		#region Properties
		internal bool IsDebuggingEnabled
		{
			get { return HttpContext.Current.IsDebuggingEnabled; }
		}

		internal ILog Logger
		{
			get { return LogManager.GetLogger("SimplicityDashboard"); }
		}
		#endregion

		#region Event Handlers
		protected void Application_Start()
		{
			BuildDependencyContainer();
			RegisterRoutes();
			BundleJavascript();
			BundleCss();

			GlobalFilters.Filters.Add(new HandleErrorAttribute());
		}
		#endregion

		#region internal virtual Methods
		internal virtual void RegisterRoutes()
		{
			var routes = RouteTable.Routes;

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Shared", action = "Index", id = UrlParameter.Optional });
			routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
		}

		internal virtual void BundleJavascript()
		{
			AddBundleToBundlesTable(ResolveFromContainer<IBundleFactory>().CreateJavascriptBundle("~/js").IncludeDirectory("~/Scripts", "*.js"));
		}

		internal virtual void BundleCss()
		{
			AddBundleToBundlesTable(ResolveFromContainer<IBundleFactory>().CreateCssBundle("~/css").IncludeDirectory("~/Style", "*.css"));
		}

		internal virtual void BuildDependencyContainer()
		{
			var builder = new ContainerBuilder();
			new AutofacRegistrar().Register(builder);

			builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly).Where(x => x.IsAssignableTo<IController>()).PropertiesAutowired();
			builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly).Where(x => x.IsAssignableTo<IHttpController>()).PropertiesAutowired();

			_container = builder.Build();

			DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));
			var autofacDependencyResolver = new AutofacDependencyResolver(_container);
			GlobalConfiguration.Configuration.DependencyResolver = new AspAutofacDependencyResolver(autofacDependencyResolver);
		}

		internal virtual void AddBundleToBundlesTable(IBundle bundle)
		{
			BundleTable.Bundles.Add(bundle.InternalBundle);
		}

		internal virtual TResolvedType ResolveFromContainer<TResolvedType>()
		{
			return _container.Resolve<TResolvedType>();
		}
		#endregion
	}
}