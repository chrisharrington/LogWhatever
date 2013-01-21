using System.Collections.Generic;
using System.Web.Http;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.MvcApplication.Controllers.Api
{
	public class TagsController : BaseApiController
	{
		#region Properties
		public ITagRepository TagRepository { get; set; }
		public ILogRepository LogRepository { get; set; }
		#endregion

		#region Public Methods
		[ActionName("log")]
		public IEnumerable<Tag> GetForLog(string logName)
		{
			var log = LogRepository.Name(logName);
			if (log == null)
				return new List<Tag>();

			return TagRepository.Log(log.Id);
		}
		#endregion
	}
}