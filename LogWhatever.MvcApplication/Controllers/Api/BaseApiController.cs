using System.Web.Http;
using LogWhatever.Common.CQRS;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.MvcApplication.Controllers.Api
{
	public class BaseApiController : ApiController
	{
		#region Properties
		public IUserRepository UserRepository { get; set; }
		public IMessageDispatcher Dispatcher { get; set; }
		#endregion

		#region Protected Methods
		protected User GetSignedInUser()
		{
			return UserRepository.Email(User.Identity.Name);
		}
		#endregion
	}
}