using System.IO;
using System.Net;
using LogWhatever.Common.Http;
using Newtonsoft.Json;

namespace LogWhatever.Service.Http
{
	public class HttpRequestor : IHttpRequestor
	{
		#region Public Methods
		public TResponseType Get<TResponseType>(string url)
		{
			var request = (HttpWebRequest) WebRequest.Create(url);
			request.Method = "GET";
			request.ContentType = "application/json";

			using (var reader = new StreamReader(request.GetResponse().GetResponseStream()))
				return JsonConvert.DeserializeObject<TResponseType>(reader.ReadToEnd());
		}
		#endregion
	}
}