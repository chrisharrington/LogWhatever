using System.Net;

namespace LogWhatever.Common.Service.Http
{
	public interface IHttpRequestor
	{
		#region Public Methods
		TResponseType Get<TResponseType>(string url, object parameters = null, params Cookie[] cookies);
		void Post(string url, object parameters = null, params Cookie[] cookies);
		#endregion
	}
}