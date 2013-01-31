using System;
using System.Collections.Generic;
using System.Linq;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.Messages.Commands;

namespace LogWhatever.Repositories
{
	public class UserRepository : BaseRepository, IUserRepository
	{
		#region Public Methods
		public virtual IEnumerable<User> All(Func<User, bool> filter = null)
		{
			return Retrieve("select * from LogUsers", filter);
		}

		public User Email(string emailAddress)
		{
			if (string.IsNullOrEmpty(emailAddress))
				throw new ArgumentNullException("emailAddress");

			return All().FirstOrDefault(x => x.EmailAddress.ToLower() == emailAddress.ToLower());
		}

		public void Create(User user)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			Dispatcher.Dispatch(AddUser.CreateFrom(user));
			Cache.AddToList(user);
		}
		#endregion
	}
}