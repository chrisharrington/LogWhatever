using System;
using System.Web.Mvc;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.MvcApplication.Controllers
{
	public class BaseController : Controller
	{
		#region Properties
		public IUserRepository UserRepository { get; set; }
		#endregion

		#region Protected Methods
		protected User GetCurrentlySignedInUser()
		{
			try
			{
				return UserRepository.Email(User.Identity.Name);
			}
			catch (Exception)
			{
				return null;
			}
		}
		#endregion
	}
}