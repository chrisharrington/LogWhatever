using System;
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
		public IEnumerable<Tag> Get(Guid? userId = null)
		{
			return TagRepository.All(x => userId == null || x.UserId == userId);
		}

		public void Post(Tag tag)
		{
			TagRepository.Create(tag);
		}
		#endregion
	}
}