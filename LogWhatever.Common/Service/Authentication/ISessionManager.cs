using LogWhatever.Common.Models;

namespace LogWhatever.Common.Service.Authentication
{
	public interface ISessionManager
	{
		#region Public Methods
		Session GetCurrent();
		#endregion
	}
}