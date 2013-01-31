using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using LogWhatever.Common.Configuration;
using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Caching;
using LogWhatever.Common.Service.Http;

namespace LogWhatever.RemoteRepositories
{
	public class BaseRemoteRepository
	{
		#region Properties
		public IHttpRequestor HttpRequestor { get; set; }
		public IConfigurationProvider Configuration { get; set; }
		public ICollectionCache Cache { get; set; }
		#endregion

		#region Public Methods
		public IEnumerable<TModel> All<TModel>(string controller, Func<TModel, bool> filter = null) where TModel : BaseModel
		{
			lock (Cache)
			{
				if (Cache.ContainsKey<TModel>())
				{
					var results = Cache.Retrieve<TModel>();
					if (filter != null)
						results = results.Where(filter);
					return results;
				}

				var session = GetCurrentSession();
				if (session == null)
					throw new HttpResponseException(HttpStatusCode.Unauthorized);

				var result = HttpRequestor.Get<IEnumerable<TModel>>(Configuration.DataServiceLocation + controller, new { auth = session.Id.ToString() });
				Cache.AddToList(result);
				if (filter != null)
					result = result.Where(filter);
				return result;
			}
		}
		#endregion

		#region Protected Methods
		protected TModel Single<TModel>(string controller, Func<TModel, bool> filter) where TModel : BaseModel
		{
			lock (Cache)
			{
				if (Cache.ContainsKey<TModel>())
					return Cache.Retrieve<TModel>().FirstOrDefault(filter);

				var session = GetCurrentSession();
				if (session == null)
					throw new HttpResponseException(HttpStatusCode.Unauthorized);

				var result = HttpRequestor.Get<IEnumerable<TModel>>(Configuration.DataServiceLocation + controller, new { auth = session.Id.ToString() });
				Cache.AddToList(result);
				return result.FirstOrDefault(filter);
			}
		}
		#endregion

		#region Private Methods
		private Session SignIn(Guid token)
		{
			var session = HttpRequestor.Get<Session>(Configuration.DataServiceLocation + "session/sign-in-token", new { token });
			if (session == null)
				return null;

			Cache.AddToList(session);
			return session;
		}

		private Session GetCurrentSession()
		{
			var token = HttpContext.Current.User.Identity.Name;
			if (string.IsNullOrEmpty(token))
				return null;

			var session = Cache.Retrieve<Session>().FirstOrDefault(x => x.Id == new Guid(token));
			if (session == null)
			{
				session = SignIn(new Guid(token));
				if (session == null)
					return null;
				Cache.AddToList(session);
			}

			return session;
		}
		#endregion
	}
}