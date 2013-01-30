using System;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Authentication;

namespace LogWhatever.DataService.Controllers
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
			return GetCurrentlySignedInUser();
		}

		[ActionName("sign-in")]
		[AcceptVerbs("GET")]
		[AllowAnonymous]
		public User SignIn(string emailAddress, string password)
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
			Cache.AddToList(new Session {Id = token, User = user});
			HttpContext.Current.Response.Cookies.Add(new HttpCookie("log-auth", token.ToString()));
			return user;
		}

		[ActionName("sign-out")]
		[AcceptVerbs("POST")]
		public void SignOut()
		{
			var token = HttpContext.Current.Request.Cookies["log-auth"];
			if (token == null)
				return;

			Cache.RemoveFromList<Session>(new Guid(token.Value));
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
			Cache.AddToList(new Session {Id = token, User = user});
			HttpContext.Current.Response.Cookies.Add(new HttpCookie("log-auth", token.ToString()));

			return user;
		}
		#endregion
	}
}