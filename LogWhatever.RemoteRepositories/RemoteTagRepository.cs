using LogWhatever.Common.Models;
using LogWhatever.Repositories;

namespace LogWhatever.RemoteRepositories
{
	public class RemoteTagRepository : TagRepository
	{
		#region Properties
		public BaseRemoteRepository Repository { get; set; } 
		#endregion

		#region Public Methods
		public override System.Collections.Generic.IEnumerable<Tag> All(System.Func<Tag, bool> filter = null)
		{
			return Repository.All("tags", filter);
		}
		#endregion
	}
}