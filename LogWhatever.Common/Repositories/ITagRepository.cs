using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Repositories
{
	public interface ITagRepository
	{
		#region Public Methods
		IEnumerable<Tag> Log(Guid logId);
		IEnumerable<Tag> User(Guid userId);
		IEnumerable<Tag> LatestForUserAndLog(Guid userId, string name);
		Tag Name(string name);
		#endregion
	}
}