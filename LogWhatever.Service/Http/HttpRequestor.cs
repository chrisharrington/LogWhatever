using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;
using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Http;
using Newtonsoft.Json;

namespace LogWhatever.Service.Http
{
	public class HttpRequestor : IHttpRequestor
	{
		#region Public Methods
		public TResponseType Get<TResponseType>(string url, object parameters = null, Session session = null)
		{
			try
			{
				var requestUriString = url + CreateUrlParameterString(parameters, session);
				var request = (HttpWebRequest) WebRequest.Create(requestUriString);
				request.Method = "GET";
				request.ContentType = "application/json";

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

		public void Post(string url, object parameters = null, Session session = null)
		{
			var request = (HttpWebRequest)WebRequest.Create(url + CreateUrlParameterString(parameters, session));
			request.Method = "POST";
			request.ContentType = "application/json";

			var serializeObject = JsonConvert.SerializeObject(parameters);
			var body = Encoding.UTF8.GetBytes(serializeObject);
			request.ContentLength = body.Length;

			using (var stream = request.GetRequestStream())
				stream.Write(body, 0, body.Length);

			var response = (HttpWebResponse)request.GetResponse();
			if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent)
				throw new HttpResponseException(response.StatusCode);
		}
		#endregion

		#region Private Methods
		internal virtual string CreateUrlParameterString(object parameters, Session session)
		{
			var result = "";
			if (parameters != null)
			{
				foreach (var property in parameters.GetType().GetProperties())
				{
					var value = property.GetValue(parameters);
					result += "&" + property.Name + "=" + (value == null ? null : HttpUtility.UrlEncode(value.ToString()));
				}
			}
			if (session != null)
				result += "&auth=" + session.Id.ToString();
			return "?" + result.Substring(1);
		}
		#endregion
	}
}