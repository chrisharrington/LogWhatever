using System.Dynamic;
using LogWhatever.Common.Configuration;

namespace LogWhatever.MvcApplication.Controllers.Api
{
	public class ConfigurationController : BaseApiController
	{
		#region Public Methods
		public IConfigurationProvider ConfigurationProvider { get; set; }
		#endregion

		#region Public Methods
		public dynamic Get()
		{
			dynamic result = new ExpandoObject();
			return result;
		}
		#endregion
	}
}