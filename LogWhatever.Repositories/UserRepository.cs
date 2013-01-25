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
		public IEnumerable<User> All()
		{
			return Retrieve<User>("select * from LogUsers");
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