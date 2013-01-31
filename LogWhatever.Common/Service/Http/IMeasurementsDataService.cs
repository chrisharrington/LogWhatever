using System.Collections.Generic;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Service.Http
{
	public interface IMeasurementsDataService
	{
		#region Public Methods
		IEnumerable<Measurement> All();
		#endregion
	}
}