using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.Repositories
{
	public class MeasurementRepository : BaseRepository, IMeasurementRepository
	{
		#region Public Methods
		public IEnumerable<Measurement> Log(Guid logId)
		{
			if (logId == Guid.Empty)
				throw new ArgumentNullException("logId");

			return Query<Measurement>("select * from Measurements where LogId = @logId", new {logId});
		}

		public IEnumerable<Measurement> User(Guid userId)
		{
			if (userId == Guid.Empty)
				throw new ArgumentNullException("userId");

			return Query<Measurement>("select * from Measurements where UserId = @userId", new {userId});
		}
		#endregion
	}
}