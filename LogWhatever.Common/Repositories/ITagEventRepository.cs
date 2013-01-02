using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Repositories
{
	public interface ITagEventRepository
	{
		#region Public Methods
		IEnumerable<TagEvent> User(Guid userId);
		IEnumerable<TagEvent> LatestForUserAndLog(Guid userId, string name);
		#endregion
	}
}