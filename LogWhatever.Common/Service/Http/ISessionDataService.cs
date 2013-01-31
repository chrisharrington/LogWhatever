using System;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Service.Http
{
	public interface ISessionDataService
	{
		#region Public Methods
		Session SignIn(string emailAddress, string password);
		Session SignIn(Guid token);
		void SignOut(Session session);
		#endregion
	}
}