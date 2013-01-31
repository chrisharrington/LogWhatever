using System.Collections.Generic;
using System.Linq;
using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Http;

namespace LogWhatever.Service.Http
{
	public class TagsDataService : BaseDataService, ITagsDataService
	{
		#region Properties
		public override string SubLocation
		{
			get { return "tags/"; }
		}
		#endregion

		#region Public Methods
		public IEnumerable<Tag> Log(Session session, string logName)
		{
			return All<Tag>().Where(x => x.LogName == logName);
		}
		#endregion
	}
}