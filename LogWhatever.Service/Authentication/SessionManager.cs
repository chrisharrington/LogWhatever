using System;
using System.Linq;
using System.Web;
using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Authentication;
using LogWhatever.Common.Service.Caching;

namespace LogWhatever.Service.Authentication
{
	public class SessionManager : ISessionManager
	{
		#region Properties
		public ICollectionCache Cache { get; set; }
		#endregion

		#region Public Methods
		public Session GetCurrent()
		{
			var token = HttpContext.Current.User.Identity.Name;
			if (string.IsNullOrEmpty(token))
				return null;

			return Cache.Retrieve<Session>().FirstOrDefault(x => x.Id == new Guid(token));
		}
		#endregion
	}
}