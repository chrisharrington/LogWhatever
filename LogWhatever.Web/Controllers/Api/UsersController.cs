using System;
using System.Web.Http;
using System.Web.Security;
using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Authentication;

namespace LogWhatever.Web.Controllers.Api
{
	public class UsersController : BaseApiController
	{
		#region Properties
		public IMembershipProvider MembershipProvider { get; set; }
		#endregion

		#region Public Methods
		[ActionName("signed-in")]
		[AllowAnonymous]
		public User GetSignedInUser()
		{
			var session = GetCurrentSession();
			return session == null ? null : session.User;
		}

		[ActionName("sign-in")]
		[AcceptVerbs("GET")]
		[AllowAnonymous]
		public User SignIn(string emailAddress, string password, bool staySignedIn)
		{
			if (string.IsNullOrEmpty(emailAddress))
				throw new ArgumentNullException("emailAddress");
			if (string.IsNullOrEmpty(password))
				throw new ArgumentNullException("password");

			var session = HttpRequestor.Get<Session>(ConfigurationProvider.DataServiceLocation + "sessions/sign-in", new { emailAddress, password });
			if (session == null)
				return null;

			Cache.AddToList(session);

			FormsAuthentication.SetAuthCookie(session.Id.ToString(), staySignedIn);
			return session.User;
		}

		[ActionName("sign-out")]
		[AcceptVerbs("POST")]
		public void SignOut()
		{
			Cache.RemoveFromList<Session>(GetCurrentSession().Id);
			HttpRequestor.Post(ConfigurationProvider.DataServiceLocation + "sign-out");
			FormsAuthentication.SignOut();
		}

		[ActionName("registration")]
		[AcceptVerbs("GET")]
		[AllowAnonymous]
		public User Register(string name, string email, string password)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (string.IsNullOrEmpty(email))
				throw new ArgumentNullException("email");
			if (string.IsNullOrEmpty(password))
				throw new ArgumentNullException("password");

			var user = new User {EmailAddress = email, Name = name, Password = password};
			UserRepository.Create(user);
			FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
			Cache.AddToList(new Session {EmailAddress = user.EmailAddress, Id = user.Id, Name = user.Name, UserId = user.Id});
			user.Password = "";
			return user;
		}
		#endregion
	}
}