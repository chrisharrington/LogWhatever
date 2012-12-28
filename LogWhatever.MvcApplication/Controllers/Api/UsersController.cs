using System;
using System.Web.Security;
using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Authentication;

namespace LogWhatever.MvcApplication.Controllers.Api
{
	public class UsersController : BaseApiController
	{
		#region Properties
		public IMembershipProvider MembershipProvider { get; set; }
		#endregion

		#region Public Methods
		[System.Web.Http.ActionName("signed-in")]
		public User GetSignedInUser()
		{
			return GetCurrentlySignedInUser();
		}

		[System.Web.Http.ActionName("sign-in")]
		[System.Web.Http.AcceptVerbs("GET")]
		public User SignIn(string emailAddress, string password, bool staySignedIn)
		{
			if (string.IsNullOrEmpty(emailAddress))
				throw new ArgumentNullException("emailAddress");
			if (string.IsNullOrEmpty(password))
				throw new ArgumentNullException("password");

			var isUserValidated = MembershipProvider.ValidateUser(emailAddress, password);
			if (!isUserValidated)
				return null;

			FormsAuthentication.SetAuthCookie(emailAddress, staySignedIn);
			return UserRepository.Email(emailAddress);
		}
		#endregion
	}
}