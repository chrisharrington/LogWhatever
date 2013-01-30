using System.Configuration;

namespace LogWhatever.Common.Configuration
{
	public class WebConfigProvider : IConfigurationProvider
	{
		#region Properties
		public ConnectionStringSettings BlobStorage
		{
			get { return ConfigurationManager.ConnectionStrings["BlobStorage"]; }
		}

		public ConnectionStringSettings ReadModelDatabase
		{
			get { return ConfigurationManager.ConnectionStrings["ReadModelDatabase"]; }
		}

		public string DataServiceLocation
		{
			get { return ConfigurationManager.AppSettings["DataServiceLocation"]; }
		}

		public string FromEmailAddress
		{
			get { return ConfigurationManager.AppSettings["FromEmailAddress"]; }
		}

		public string SendGridUserName
		{
			get { return ConfigurationManager.AppSettings["SendGridUserName"]; }
		}

		public string SendGridPassword
		{
			get { return ConfigurationManager.AppSettings["SendGridPassword"]; }
		}
		#endregion
		
	}
}