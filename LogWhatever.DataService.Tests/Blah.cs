using System;
using System.Net;
using System.Text;
using Xunit;

namespace LogWhatever.DataService.Tests
{
	public class Blah
	{
		[Fact]
 		public void ShouldBlah()
		{
			var request = (HttpWebRequest) WebRequest.Create(@"http://localhost:60227/events?auth=" + Guid.NewGuid());

			var encoding = new ASCIIEncoding();
			var data = encoding.GetBytes("username=user&password=pass");

			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = data.Length;

			using (var stream = request.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
			}

			request.GetResponse();
		}
	}
}