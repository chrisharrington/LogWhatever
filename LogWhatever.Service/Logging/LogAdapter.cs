using System;
using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Logging;
using log4net;

namespace LogWhatever.Service.Logging
{
	public class LogAdapter : ILogger
	{
		#region Properties
		public ILog Logger
		{
			get { return LogManager.GetLogger("SimplicityDashboard"); }
		}
		#endregion

		#region Public Methods
		public void Info(string message, User user = null, Exception exception = null)
		{
			Logger.Info(BuildMessage(message, user), exception);
		}

		public void Warn(string message, User user = null, Exception exception = null)
		{
			Logger.Warn(BuildMessage(message, user), exception);
		}

		public void Error(string message, User user = null, Exception exception = null)
		{
			Logger.Error(BuildMessage(message, user), exception);
		}
		#endregion

		#region Private Methods
		internal virtual string BuildMessage(string message, User user)
		{
			return user == null ? message : (user.EmailAddress + " - " + message);
		}
		#endregion
	}
}