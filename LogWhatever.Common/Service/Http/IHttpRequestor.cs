using System.Net;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Service.Http
{
	public interface IHttpRequestor
	{
		#region Public Methods
		TResponseType Get<TResponseType>(string url, object parameters = null, Session session = null);
		void Post(string url, object parameters = null, Session session = null);
		#endregion
	}
}