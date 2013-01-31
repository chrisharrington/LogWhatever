using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using LogWhatever.Common.Configuration;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.Common.Service.Authentication;
using LogWhatever.Common.Service.Caching;
using LogWhatever.Common.Service.Http;

namespace LogWhatever.Web.Controllers.Api
{
	[WebAuthorize]
	public class BaseApiController : ApiController
	{
		#region Properties
		public IUserRepository UserRepository { get; set; }
		public ICollectionCache Cache { get; set; }
		public IDataService DataService { get; set; }
		public IHttpRequestor HttpRequestor { get; set; }
		public IConfigurationProvider ConfigurationProvider { get; set; }
		#endregion

		#region Protected Methods
		protected Session GetCurrentSession()
		{
			var token = User.Identity.Name;
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

		protected IEnumerable<TModel> All<TModel>(string controller, Func<TModel, bool> filter = null) where TModel : BaseModel
		{
			lock (Cache)
			{
				if (Cache.ContainsKey<TModel>())
					return Cache.Retrieve<TModel>();

				var session = GetCurrentSession();
				if (session == null)
					throw new HttpResponseException(HttpStatusCode.Unauthorized);

				var result = HttpRequestor.Get<IEnumerable<TModel>>(ConfigurationProvider.DataServiceLocation + controller, new { auth = session.Id.ToString() });
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
			var session = HttpRequestor.Get<Session>(ConfigurationProvider.DataServiceLocation + "sessions/sign-in-token", new { token });
			if (session == null)
				return null;

			Cache.AddToList(session);
			return session;
		}
		#endregion
	}
}