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
		public IEnumerable<Tag> LogId(Guid logId)
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

		public Tag Name(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			return Query<Tag>("select * from Tags where Name = @name", new {name}).FirstOrDefault();
		}
		#endregion
	}
}