using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Repositories
{
	public interface IMeasurementRepository
	{
		#region Public Methods
		IEnumerable<Measurement> Log(Guid logId);
		IEnumerable<Measurement> User(Guid userId);
		#endregion
	}
}