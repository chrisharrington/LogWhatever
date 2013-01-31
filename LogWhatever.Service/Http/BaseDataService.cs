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

namespace LogWhatever.Service.Http
{
	public abstract class BaseDataService
	{
		#region Properties
		public IHttpRequestor HttpRequestor { get; set; }
		public IConfigurationProvider Configuration { get; set; }
		public ICollectionCache Cache { get; set; }

		public abstract string SubLocation { get; }
		public string DataServiceLocation
		{
			get { return Configuration.DataServiceLocation + SubLocation; }
		}
		#endregion

		#region Protected Methods
		protected IEnumerable<TModel> All<TModel>(Func<TModel, bool> filter = null) where TModel : BaseModel
		{
			lock (Cache)
			{
				if (Cache.ContainsKey<TModel>())
					return Cache.Retrieve<TModel>();

				var session = GetCurrentSession();
				if (session == null)
					throw new HttpResponseException(HttpStatusCode.Unauthorized);

				var result = HttpRequestor.Get<IEnumerable<TModel>>(DataServiceLocation, new { auth = session.Id.ToString() });
				Cache.AddToList(result);
				if (filter != null)
					result = result.Where(filter);
				return result;
			}
		}
		#endregion

		#region Private Methods
		private Session SignIn(Guid token)
		{
			var session = HttpRequestor.Get<Session>(DataServiceLocation + "sign-in-token", new { token });
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