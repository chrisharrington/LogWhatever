using System.Collections.Generic;
using System.Web.Http;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.Web.Controllers.Api
{
	public class TagsController : BaseApiController
	{
		#region Properties
		public ITagRepository TagRepository { get; set; }
		#endregion

		#region Public Methods
		[ActionName("log")]
		public IEnumerable<Tag> GetForLog(string logName)
		{
			return TagRepository.All(x => x.LogName == logName);
		}
		#endregion
	}
}