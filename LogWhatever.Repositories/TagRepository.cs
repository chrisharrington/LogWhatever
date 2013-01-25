using System;
using System.Collections.Generic;
using System.Linq;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.Messages.Commands;

namespace LogWhatever.Repositories
{
	public class TagRepository : BaseRepository, ITagRepository
	{
		#region Public Methods
		public IEnumerable<Tag> All()
		{
			return Retrieve<Tag>("select * from Tags");
		}

		public IEnumerable<Tag> Log(Guid logId)
		{
			if (logId == Guid.Empty)
				throw new ArgumentNullException("logId");

			return All().Where(x => x.LogId == logId);
		}

		public IEnumerable<Tag> User(Guid userId)
		{
			if (userId == Guid.Empty)
				throw new ArgumentNullException("userId");

			return All().Where(x => x.UserId == userId);
		}

		public IEnumerable<Tag> LatestForUserAndLog(Guid userId, string name)
		{
			if (userId == Guid.Empty)
				throw new ArgumentNullException("userId");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			var tags = User(userId).Where(x => x.LogName == name);
			return !tags.Any() ? (IEnumerable<Tag>) new List<Tag>() : tags.GroupBy(x => x.Date).OrderByDescending(x => x.Key).First();
		}

		public Tag Name(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			return All().FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
		}

		public void Create(Tag tag)
		{
			if (tag == null)
				throw new ArgumentNullException("tag");

			Dispatcher.Dispatch(AddTag.CreateFrom(tag));
			Cache.AddToList(tag);
		}
		#endregion
	}
}