using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Repositories
{
	public interface IMeasurementRepository
	{
		#region Public Methods
		IEnumerable<Measurement> LogId(Guid logId);
		#endregion
	}
}