using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using LogWhatever.Container;

namespace LogWhatever.DataService
{
	public class MvcApplication : System.Web.HttpApplication
	{
		#region Data Members
		internal IContainer _container;
		#endregion

		#region Event Handlers
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterRoutes();
			BuildDependencyContainer();

			GlobalFilters.Filters.Add(new HandleErrorAttribute());
		}
		#endregion

		#region Private Methods
		internal virtual void RegisterRoutes()
		{
			var routes = GlobalConfiguration.Configuration.Routes;
			routes.MapHttpRoute("DefaultApi", "{controller}/{id}", new { id = RouteParameter.Optional });
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
		#endregion
	}
}