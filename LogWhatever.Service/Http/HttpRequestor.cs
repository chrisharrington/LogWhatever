using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Http;
using LogWhatever.Common.Service.Http;
using Newtonsoft.Json;

namespace LogWhatever.Service.Http
{
	public class HttpRequestor : IHttpRequestor
	{
		#region Public Methods
		public TResponseType Get<TResponseType>(string url, object parameters = null, params Cookie[] cookies)
		{
			try
			{
				var requestUriString = url + (parameters == null ? "" : CreateUrlParameterString(parameters));
				var request = (HttpWebRequest) WebRequest.Create(requestUriString);
				request.Method = "GET";
				request.ContentType = "application/json";

				AddCookiesToRequest(request, cookies);

				var response = (HttpWebResponse) request.GetResponse();
				if (response.StatusCode != HttpStatusCode.OK)
					throw new HttpResponseException(response.StatusCode);

				using (var reader = new StreamReader(response.GetResponseStream()))
					return JsonConvert.DeserializeObject<TResponseType>(reader.ReadToEnd());
			}
			catch (WebException ex)
			{
				throw new HttpResponseException(((HttpWebResponse) ex.Response).StatusCode);
			}
		}

		private void AddCookiesToRequest(HttpWebRequest request, IEnumerable<Cookie> cookies)
		{
			if (cookies == null)
				return;

			if (request.CookieContainer == null)
				request.CookieContainer = new CookieContainer();
			foreach (var cookie in cookies)
				request.CookieContainer.Add(cookie);
		}

		public void Post(string url, object parameters = null, params Cookie[] cookies)
		{
			var request = (HttpWebRequest) WebRequest.Create(url + (parameters == null ? "" : CreateUrlParameterString(parameters)));
			request.Method = "POST";
			request.ContentType = "application/json";

			AddCookiesToRequest(request, cookies);

			var response = (HttpWebResponse) request.GetResponse();
			if (response.StatusCode != HttpStatusCode.OK)
				throw new HttpResponseException(response.StatusCode);
		}
		#endregion

		#region Private Methods
		internal virtual string CreateUrlParameterString(object parameters)
		{
			var result = "";
			foreach (var property in parameters.GetType().GetProperties())
				result += "&" + property.Name + "=" + HttpUtility.UrlEncode(property.GetValue(parameters).ToString());
			return "?" + result.Substring(1);
		}
		#endregion
	}
}