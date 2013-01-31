using System.Dynamic;
using System.Web.Http;

namespace LogWhatever.Web.Controllers.Api
{
	public class ConfigurationController : BaseApiController
	{
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