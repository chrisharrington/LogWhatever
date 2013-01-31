using System;
using System.Collections.Generic;
using System.Linq;
using LogWhatever.Common.Models;
using LogWhatever.Common.Repositories;
using LogWhatever.Messages.Commands;

namespace LogWhatever.Repositories
{
	public class MeasurementRepository : BaseRepository, IMeasurementRepository
	{
		#region Public Methods
		public virtual IEnumerable<Measurement> All(Func<Measurement, bool> filter = null)
		{
			return Retrieve("select * from Measurements", filter);
		}

		public IEnumerable<Measurement> Log(Guid logId)
		{
			if (logId == Guid.Empty)
				throw new ArgumentNullException("logId");

			return All().Where(x => x.LogId == logId);
		}

		public IEnumerable<Measurement> User(Guid userId)
		{
			if (userId == Guid.Empty)
				throw new ArgumentNullException("userId");

			return All().Where(x => x.UserId == userId);
		}

		public void Create(Measurement measurement)
		{
			if (measurement == null)
				throw new ArgumentNullException("measurement");

			Dispatcher.Dispatch(AddMeasurement.CreateFrom(measurement));
			Cache.AddToList(measurement);
		}
		#endregion
	}
}