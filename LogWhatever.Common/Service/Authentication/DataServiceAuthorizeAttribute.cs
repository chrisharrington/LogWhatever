using System;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Caching;
using AllowAnonymousAttribute = System.Web.Http.AllowAnonymousAttribute;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;

namespace LogWhatever.Common.Service.Authentication
{
	public class DataServiceAuthorizeAttribute : AuthorizeAttribute
	{
		#region Properties
		private ICollectionCache Cache
		{
			get { return DependencyResolver.Current.GetService<ICollectionCache>(); }
		}
		#endregion

		#region Public Methods
		protected override bool IsAuthorized(HttpActionContext context)
		{
			if (context.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
				return true;

			var parts = HttpUtility.ParseQueryString(context.Request.RequestUri.Query);
			if (parts.AllKeys.All(x => x != "auth"))
				return false;

			var token = parts["auth"];
			if (string.IsNullOrEmpty(token))
				return false;

			Guid tokenId;
			if (!Guid.TryParse(token, out tokenId))
				return false;

			return Cache.Retrieve<Session>().Any(x => x.Id == tokenId);
		}
		#endregion
	}
}