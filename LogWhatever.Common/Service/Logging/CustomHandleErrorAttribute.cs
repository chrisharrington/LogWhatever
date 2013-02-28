using System.Web.Mvc;
using LogWhatever.Common.Service.Authentication;

namespace LogWhatever.Common.Service.Logging
{
	public class CustomHandleErrorAttribute : HandleErrorAttribute
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
		public override void OnException(ExceptionContext context)
		{
			var session = SessionManager.GetCurrent();
			Logger.Error("An unhandled error has occurred during " + context.RouteData + ".", session == null ? null : session.User, context.Exception);
			base.OnException(context);
		}
		#endregion
	}
}