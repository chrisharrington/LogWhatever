using System;
using System.Collections.Generic;
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

			return Query<Tag>("select * from Tags where Log = @logId", new {logId});
		}

		public IEnumerable<Tag> User(Guid userId)
		{
			if (userId == Guid.Empty)
				throw new ArgumentNullException("userId");

			return Query<Tag>("select * from Tags where UserId = @userId", new {userId});
		}
		#endregion
	}
}