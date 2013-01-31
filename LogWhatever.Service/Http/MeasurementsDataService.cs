using System.Collections.Generic;
using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Http;

namespace LogWhatever.Service.Http
{
	public class MeasurementsDataService : BaseDataService, IMeasurementsDataService
	{
		#region Properties
		public override string SubLocation
		{
			get { return "measurements/"; }
		}
		#endregion

		#region Public Methods
		public IEnumerable<Measurement> All()
		{
			return All<Measurement>();
		}
		#endregion
	}
}