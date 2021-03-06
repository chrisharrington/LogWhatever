﻿using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.Common.Service.Authentication;
using LogWhatever.Common.Service.Caching;
using LogWhatever.Common.Service.Logging;

namespace LogWhatever.DataService.Controllers
{
	[DataServiceAuthorize]
	[CustomHandleError]
	[CustomHandleAction]
	public class BaseApiController : ApiController
	{
		#region Properties
		public IUserRepository UserRepository { get; set; }
		public ICollectionCache Cache { get; set; }
		public ILogger Logger { get; set; }
		#endregion

		#region Protected Methods
		protected User GetCurrentlySignedInUser()
		{
			var token = HttpContext.Current.Request.Cookies["log-auth"];
			if (token == null)
				return null;

			var tokenId = new Guid(token.Value);
			var session = Cache.Retrieve<Session>().FirstOrDefault(x => x.Id == tokenId);
			return session == null ? null : session.User;
		}
		#endregion
	}
}