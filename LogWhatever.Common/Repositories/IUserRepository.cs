using LogWhatever.Common.Models;

namespace LogWhatever.Common.Repositories
{
	public interface IUserRepository : IRepository<User>
	{
		#region Public Methods
		User Email(string emailAddress);
		void Create(User user);
		#endregion
	}
}