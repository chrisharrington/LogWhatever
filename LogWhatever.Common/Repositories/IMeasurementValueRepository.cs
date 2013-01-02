using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Repositories
{
	public interface IMeasurementValueRepository
	{
		#region Public Methods
		IEnumerable<MeasurementValue> Log(Guid logId);
		IEnumerable<MeasurementValue> User(Guid userId);
		#endregion
	}
}