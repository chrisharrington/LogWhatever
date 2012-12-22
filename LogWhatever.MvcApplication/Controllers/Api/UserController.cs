using System;
using System.Web.Http;
using LogWhatever.Common.Models;

namespace LogWhatever.MvcApplication.Controllers.Api
{
	public class UserController : BaseApiController
	{
		#region Public Methods
		[ActionName("signed-in")]
		public User GetSignedInUser()
		{
			return new User {EmailAddress = "chrisharrington99@gmail.com", Id = Guid.NewGuid(), Name = "Chris Harrington"};
		}
		#endregion
	}
}