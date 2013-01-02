using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.Repositories
{
	public class TagEventRepository : BaseRepository, ITagEventRepository
	{
		#region Public Methods
		public IEnumerable<TagEvent> User(Guid userId)
		{
			if (userId == Guid.Empty)
				throw new ArgumentNullException("userId");

			return Query<TagEvent>("select * from TagEvents where UserId = @userId", new {userId});
		}

		public IEnumerable<TagEvent> LatestForUserAndLog(Guid userId, string name)
		{
			if (userId == Guid.Empty)
				throw new ArgumentNullException("userId");

			return Query<TagEvent>("select * from TagEvents where EventId = (select top 1 EventId from TagEvents where UserId = @userId and LogName = @name order by UpdatedDate desc)", new { userId, name });
		}
		#endregion
	}
}