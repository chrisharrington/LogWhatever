using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Repositories
{
	public interface ITagRepository
	{
		#region Public Methods
		IEnumerable<Tag> LogId(Guid logId);
		#endregion
	}
}