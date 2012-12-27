using System.Web;
using LogWhatever.Common.Configuration;
using LogWhatever.Common.Service.Email;
using LogWhatever.Service.Email;
using log4net.Appender;
using log4net.Core;

namespace LogWhatever.Service.Logging
{
	public class EmailAppender : AppenderSkeleton
	{
		#region Properties
		public virtual IEmailer Emailer { get; set; }
		public virtual IConfigurationProvider ConfigurationProvider { get; set; }
		public string Recipients{ get; set; }
		#endregion

		#region Constructors
		public EmailAppender()
		{
			ConfigurationProvider = new WebConfigProvider();
			Emailer = new SendGridEmailer(ConfigurationProvider);
		}
		#endregion

		#region Protected Methods
		protected override void Append(LoggingEvent loggingEvent)
		{
			if (IsDebuggingEnabled())
				return;

			Emailer.Send("error@LogWhatever.com", Recipients, "Simplicity Dashboard - Error", CreateBody(loggingEvent));
		}
		#endregion

		#region Private Methods
		internal virtual string CreateBody(LoggingEvent loggingEvent)
		{
			return "Hi there!<br><br>An unhandled exception has occurred during regular operation of the Simplicity Dashboard application. The details of the error are as follows:<br><br>" + GetRenderedLoggedEvent(loggingEvent).Replace("\n", "<br>") + "<br><br>Good luck!<br>The Simplicity Dashboard Team";
		}

		internal virtual string GetRenderedLoggedEvent(LoggingEvent loggingEvent)
		{
			return RenderLoggingEvent(loggingEvent);
		}

		internal virtual bool IsDebuggingEnabled()
		{
			return HttpContext.Current.IsDebuggingEnabled;
		}
		#endregion
	}
}