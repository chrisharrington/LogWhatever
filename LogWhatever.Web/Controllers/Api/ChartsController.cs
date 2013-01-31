using System.Web.Http;

namespace LogWhatever.Web.Controllers.Api
{
	public class ChartsController : BaseApiController
	{
		#region Public Methods
		public dynamic Get([FromUri] string logName)
		{
			return HttpRequestor.Get<object>(ConfigurationProvider.DataServiceLocation + "charts", new { logName, auth = GetCurrentSession().Id.ToString() });
		}
		#endregion
	}
}