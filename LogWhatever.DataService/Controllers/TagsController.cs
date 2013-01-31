using System.Collections.Generic;
using System.Web.Http;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.DataService.Controllers
{
	public class TagsController : BaseApiController
	{
		#region Properties
		public ITagRepository TagRepository { get; set; }
		public ILogRepository LogRepository { get; set; }
		#endregion

		#region Public Methods
		public IEnumerable<Tag> Get()
		{
			return TagRepository.All();
		}
		#endregion
	}
}