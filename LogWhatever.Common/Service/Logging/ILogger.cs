using System;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Service.Logging
{
	public interface ILogger
	{
		#region Public Methods
		void Info(string message, User user = null, Exception exception = null);
		void Warn(string message, User user = null, Exception exception = null);
		void Error(string message, User user = null, Exception exception = null);
		#endregion
	}
}