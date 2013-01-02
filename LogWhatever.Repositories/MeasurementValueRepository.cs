using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.Repositories
{
	public class MeasurementValueRepository : BaseRepository, IMeasurementValueRepository
	{
		#region Public Methods
		public IEnumerable<MeasurementValue> Log(Guid logId)
		{
			if (logId == null)
				throw new ArgumentNullException("logId");

			return Query<MeasurementValue>("select * from MeasurementValues where LogId = @logId", new {logId});
		}

		public IEnumerable<MeasurementValue> User(Guid userId)
		{
			if (userId == Guid.Empty)
				throw new ArgumentNullException("userId");

			return Query<MeasurementValue>("select * from MeasurementValues where UserId = @userId", new {userId});
		}
		#endregion
	}
}