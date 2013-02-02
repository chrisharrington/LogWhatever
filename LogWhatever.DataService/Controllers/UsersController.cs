using System.Web.Http;
using System.Web.Security;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.Common.Service.Authentication;

namespace LogWhatever.DataService.Controllers
{
	public class UsersController : BaseApiController
	{
		#region Properties
		public IMembershipProvider MembershipProvider { get; set; }
		public ISessionRepository SessionRepository { get; set; }
		#endregion

		#region Public Methods
		[AllowAnonymous]
		public void Post(User user)
		{
			MembershipCreateStatus status;
			MembershipProvider.CreateUser(user.EmailAddress, user.Password, user.EmailAddress, "User", null, null, true, null, out status);
			if (status != MembershipCreateStatus.Success)
				throw new MembershipCreateUserException(status);

			UserRepository.Create(user);
			SessionRepository.Create(new Session { Id = user.Id, UserId = user.Id, EmailAddress = user.EmailAddress, Name = user.Name });
		}
		#endregion
	}
}