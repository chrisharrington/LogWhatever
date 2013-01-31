using System;
using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Http;

namespace LogWhatever.Service.Http
{
	public class SessionsDataService : BaseDataService, ISessionDataService
	{
		#region Properties
		public override string SubLocation
		{
			get { return "sessions/"; }
		}
		#endregion

		#region Public Methods
		public Session SignIn(string emailAddress, string password)
		{
			var session = HttpRequestor.Get<Session>(DataServiceLocation + "sign-in", new {emailAddress, password});
			if (session == null)
				return null;

			Cache.AddToList(session);
			return session;
		}

		public Session SignIn(Guid token)
		{
			var session = HttpRequestor.Get<Session>(DataServiceLocation + "sign-in-token", new {token});
			if (session == null)
				return null;

			Cache.AddToList(session);
			return session;
		}

		public void SignOut(Session session)
		{
			Cache.RemoveFromList<Session>(session.Id);
			HttpRequestor.Post(DataServiceLocation + "sign-out");
		}
		#endregion
	}
}