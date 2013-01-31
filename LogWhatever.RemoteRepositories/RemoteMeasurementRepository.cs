using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;
using LogWhatever.Repositories;

namespace LogWhatever.RemoteRepositories
{
	public class RemoteMeasurementRepository : MeasurementRepository
	{
		#region Properties
		public BaseRemoteRepository Repository { get; set; }
		#endregion

		#region Public Methods
		public override IEnumerable<Measurement> All(Func<Measurement, bool> filter = null)
		{
			return Repository.All("measurements", filter);
		}
		#endregion
	}
}