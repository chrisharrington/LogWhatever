using System.Dynamic;
using System.Web.Http;
using LogWhatever.Common.Configuration;

namespace LogWhatever.DataService.Controllers
{
	public class ConfigurationController : BaseApiController
	{
		#region Public Methods
		public IConfigurationProvider ConfigurationProvider { get; set; }
		#endregion

		#region Public Methods
		[AllowAnonymous]
		public dynamic Get()
		{
			dynamic result = new ExpandoObject();
			return result;
		}
		#endregion
	}
}