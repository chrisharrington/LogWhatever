using System;
using System.Web.Http;
using LogWhatever.Common.Models;

namespace LogWhatever.MvcApplication.Controllers.Api
{
	public class UsersController : BaseApiController
	{
		#region Public Methods
		[ActionName("signed-in")]
		public User GetSignedInUser()
		{
			return new User {EmailAddress = "chrisharrington99@gmail.com", Id = Guid.NewGuid(), Name = "Chris Harrington"};
		}

		[ActionName("sign-in")]
		[AcceptVerbs("GET")]
		public User SignIn(string emailAddress, string password)
		{
			return null;
		}
		#endregion
	}
}