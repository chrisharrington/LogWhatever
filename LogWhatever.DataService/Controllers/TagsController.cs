using System.Collections.Generic;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.DataService.Controllers
{
	public class TagsController : BaseApiController
	{
		#region Properties
		public ITagRepository TagRepository { get; set; }
		#endregion

		#region Public Methods
		public IEnumerable<Tag> Get()
		{
			return TagRepository.All();
		}

		public void Post(Tag tag)
		{
			TagRepository.Create(tag);
		}
		#endregion
	}
}