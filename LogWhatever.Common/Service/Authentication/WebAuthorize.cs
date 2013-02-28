using System;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Caching;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;

namespace LogWhatever.Common.Service.Authentication
{
	public class WebAuthorize : AuthorizeAttribute
	{
		#region Properties
		private ICollectionCache Cache
		{
			get { return DependencyResolver.Current.GetService<ICollectionCache>(); }
		}	
		#endregion

		#region Protected Methods
		protected override bool IsAuthorized(HttpActionContext context)
		{
			var token = HttpContext.Current.User.Identity.Name;
			if (string.IsNullOrEmpty(token))
				return false;

			var tokenId = new Guid(token);
			var session = Cache.Retrieve<Session>().FirstOrDefault(x => x.Id == tokenId);
			return session != null;
		}
		#endregion	 
	}
}