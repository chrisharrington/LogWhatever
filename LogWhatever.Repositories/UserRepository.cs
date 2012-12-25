using System;
using System.Linq;
using Dapper;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.Repositories
{
	public class UserRepository : BaseRepository, IUserRepository
	{
		#region Public Methods
		public User GetByEmail(string emailAddress)
		{
			if (string.IsNullOrEmpty(emailAddress))
				throw new ArgumentNullException("emailAddress");

			using (var connection = OpenConnection())
			{
				return connection.Query<User>("select * from LogUsers where EmailAddress = @emailAddress", new { emailAddress }).FirstOrDefault();
			}
		}
		#endregion
	}
}