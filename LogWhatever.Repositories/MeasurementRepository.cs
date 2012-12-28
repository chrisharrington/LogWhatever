using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;

namespace LogWhatever.Repositories
{
	public class MeasurementRepository : BaseRepository, IMeasurementRepository
	{
		#region Public Methods
		public IEnumerable<Measurement> LogId(Guid logId)
		{
			if (logId == Guid.Empty)
				throw new ArgumentNullException("logId");

			return Query<Measurement>("select * from Measurements where LogId = @logId", new {logId});
		}
		#endregion
	}
}