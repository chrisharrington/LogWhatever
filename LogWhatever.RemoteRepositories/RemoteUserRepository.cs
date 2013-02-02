using LogWhatever.Common.Models;
using LogWhatever.Repositories;

namespace LogWhatever.RemoteRepositories
{
	public class RemoteUserRepository : UserRepository
	{
		#region Properties
		public BaseRemoteRepository Repository { get; set; }
		#endregion

		#region Public Methods
		public override System.Collections.Generic.IEnumerable<User> All(System.Func<User, bool> filter = null)
		{
			return Repository.All("users", filter);
		}

		public override void Create(User user)
		{
			Repository.HttpRequestor.Post(ConfigurationProvider.DataServiceLocation + "users", user);
		}
		#endregion
	}
}