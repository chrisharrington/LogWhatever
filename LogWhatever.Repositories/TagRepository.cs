using System;
using System.Collections.Generic;
using System.Linq;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.Repositories
{
	public class TagRepository : BaseRepository, ITagRepository
	{
		#region Public Methods
		public IEnumerable<Tag> Log(Guid logId)
		{
			if (logId == Guid.Empty)
				throw new ArgumentNullException("logId");

			return Query<Tag>("select * from Tags where LogId = @logId", new {logId});
		}

		public IEnumerable<Tag> User(Guid userId)
		{
			if (userId == Guid.Empty)
				throw new ArgumentNullException("userId");

			return Query<Tag>("select * from Tags where UserId = @userId", new {userId});
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

			return Query<Tag>("select * from Tags where Name = @name", new {name}).FirstOrDefault();
		}
		#endregion
	}
}