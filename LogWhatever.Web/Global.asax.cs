﻿using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Autofac;
using Autofac.Integration.Mvc;
using LogWhatever.Common.Adapters.Bundling;
using LogWhatever.Common.Models;
using LogWhatever.Container;
using LogWhatever.RemoteRepositories;
using log4net;
using log4net.Config;

namespace LogWhatever.Web
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
			get { return LogManager.GetLogger("LogWhatever"); }
		}
		#endregion

		#region Event Handlers
		protected void Application_Start()
		{
			XmlConfigurator.Configure();

			Logger.Info("----------------------------------------------");
			Logger.Info("Application starting.");

			BuildDependencyContainer();
			RegisterRoutes();
			BundleJavascript();
			BundleCss();
			EnsureRolesExist();

			GlobalFilters.Filters.Add(new HandleErrorAttribute());

			Logger.Info("Application start finished.");
		}
		#endregion

		#region internal virtual Methods
		internal virtual void RegisterRoutes()
		{
			Logger.Info("Registering routes...");

			var routes = RouteTable.Routes;

			routes.MapHttpRoute("ControllerAndAction", "api/{controller}/{action}", new { action = RouteParameter.Optional });
			routes.MapRoute("Details", "pages/details/{name}", new { controller = "Pages", action = "Details" });
			routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Shared", action = "Index", id = UrlParameter.Optional });

			Logger.Info("Routes registered.");
		}

		internal virtual void BundleJavascript()
		{
			AddBundleToBundlesTable(ResolveFromContainer<IBundleFactory>().CreateJavascriptBundle("~/js")
				.IncludeDirectory("~/Scripts/Prereqs", "*.js")
				.IncludeDirectory("~/Scripts/Extensions", "*.js")
				.IncludeDirectory("~/Scripts/ThirdParty", "*.js")
				.IncludeDirectory("~/Scripts/Plugins", "*.js")
				.IncludeDirectory("~/Scripts/Controls", "*.js")
				.IncludeDirectory("~/Scripts/Routers", "*.js"));
			Logger.Info("Bundled javascript.");
		}

		internal virtual void BundleCss()
		{
			AddBundleToBundlesTable(ResolveFromContainer<IBundleFactory>().CreateCssBundle("~/css").IncludeDirectory("~/Style", "*.css"));
			Logger.Info("Bundled css.");
		}

		internal virtual void BuildDependencyContainer()
		{
			var builder = new ContainerBuilder();
			new AutofacRegistrar().Register(builder);

			builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly).Where(x => x.IsAssignableTo<IController>()).PropertiesAutowired();
			builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly).Where(x => x.IsAssignableTo<IHttpController>()).PropertiesAutowired();
			builder.RegisterAssemblyTypes(typeof(BaseRemoteRepository).Assembly).AsImplementedInterfaces().PropertiesAutowired();
			builder.RegisterInstance(new BaseRemoteRepository()).PropertiesAutowired();

			_container = builder.Build();

			DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));
			var autofacDependencyResolver = new AutofacDependencyResolver(_container);
			GlobalConfiguration.Configuration.DependencyResolver = new AspAutofacDependencyResolver(autofacDependencyResolver);

			Logger.Info("Built dependency container.");
		}

		internal virtual void AddBundleToBundlesTable(IBundle bundle)
		{
			BundleTable.Bundles.Add(bundle.InternalBundle);
		}

		internal virtual TResolvedType ResolveFromContainer<TResolvedType>()
		{
			return _container.Resolve<TResolvedType>();
		}

		internal void EnsureRolesExist()
		{
			var existingRoles = Roles.GetAllRoles();
			foreach (var role in new[] { "Administrator", "User", "Read Only" }.Where(role => existingRoles.All(x => x != role)))
				Roles.CreateRole(role);
			Logger.Info("Ensured user roles exist.");
		}
		#endregion
	}
}