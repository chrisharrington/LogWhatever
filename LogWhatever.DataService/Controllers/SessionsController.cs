using System;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.Common.Service.Authentication;
using LogWhatever.Common.Service.Logging;

namespace LogWhatever.DataService.Controllers
{
	public class SessionsController : BaseApiController
	{
		#region Properties
		public IMembershipProvider MembershipProvider { get; set; }
		public ISessionRepository SessionRepository { get; set; }
		#endregion

		#region Public Methods
		[ActionName("signed-in")]
		[AllowAnonymous]
		public User GetSignedInUser()
		{
			return GetCurrentlySignedInUser();
		}

		[ActionName("sign-in-token")]
		[AcceptVerbs("GET")]
		[AllowAnonymous]
		public Session SignInToken(Guid token)
		{
			if (token == Guid.Empty)
				throw new ArgumentNullException("token");

			return SessionRepository.Id(token);
		}

		[ActionName("sign-in")]
		[AcceptVerbs("GET")]
		[AllowAnonymous]
		public Session SignIn(string emailAddress, string password)
		{
			if (string.IsNullOrEmpty(emailAddress))
				throw new ArgumentNullException("emailAddress");
			if (string.IsNullOrEmpty(password))
				throw new ArgumentNullException("password");

			var isUserValidated = MembershipProvider.ValidateUser(emailAddress, password);
			if (!isUserValidated)
				return null;

			var token = Guid.NewGuid();
			var user = UserRepository.Email(emailAddress);
			var session = new Session { Id = token, UserId = user.Id, EmailAddress = user.EmailAddress, Name = user.Name };
			SessionRepository.Create(session);
			HttpContext.Current.Response.Cookies.Add(new HttpCookie("log-auth", token.ToString()));
			return session;
		}

		[ActionName("sign-out")]
		[AcceptVerbs("POST")]
		public void SignOut()
		{
			var token = HttpContext.Current.Request.Cookies["log-auth"];
			if (token == null)
				return;

			SessionRepository.Delete(new Guid(token.Value));
		}

		[ActionName("registration")]
		[AcceptVerbs("GET")]
		public User Register(string name, string email, string password)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (string.IsNullOrEmpty(email))
				throw new ArgumentNullException("email");
			if (string.IsNullOrEmpty(password))
				throw new ArgumentNullException("password");

			MembershipCreateStatus status;
			MembershipProvider.CreateUser(email, password, email, "User", null, null, true, null, out status);
			if (status != MembershipCreateStatus.Success)
				throw new MembershipCreateUserException(status);

			var user = new User { Name = name, EmailAddress = email };
			UserRepository.Create(user);

			var token = Guid.NewGuid();
			Cache.AddToList(new Session {Id = token, UserId = user.Id, EmailAddress = user.EmailAddress, Name = user.Name});
			HttpContext.Current.Response.Cookies.Add(new HttpCookie("log-auth", token.ToString()));

			return user;
		}
		#endregion
	}
}