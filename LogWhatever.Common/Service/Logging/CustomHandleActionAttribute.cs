using System.Linq;
using System.Web.Mvc;
using LogWhatever.Common.Service.Authentication;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace LogWhatever.Common.Service.Logging
{
	public class CustomHandleActionAttribute : ActionFilterAttribute
	{
		#region Properties
		public ILogger Logger
		{
			get { return DependencyResolver.Current.GetService<ILogger>(); }
		}

		public ISessionManager SessionManager
		{
			get { return DependencyResolver.Current.GetService<ISessionManager>(); }
		}
		#endregion

		#region Public Methods
		public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext context)
		{
			Logger.Info(context.ControllerContext.RouteData.Route + ": " + context.ActionArguments.Values.Aggregate((first, second) => first + ", " + second));
			base.OnActionExecuting(context);
		}

		public override void OnActionExecuted(System.Web.Http.Filters.HttpActionExecutedContext context)
		{
			base.OnActionExecuted(context);
		}
		#endregion
	}
}