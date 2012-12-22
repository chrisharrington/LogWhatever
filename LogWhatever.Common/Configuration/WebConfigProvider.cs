using System;
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

		public string FogbugzUserName
		{
			get { return ConfigurationManager.AppSettings["FogbugzUserName"]; }
		}

		public string FogbugzPassword
		{
			get { return ConfigurationManager.AppSettings["FogbugzPassword"]; }
		}

		public string FogbugzAPILocation
		{
			get { return ConfigurationManager.AppSettings["FogbugzAPILocation"]; }
		}

		public string SmtpAddress
		{
			get { return ConfigurationManager.AppSettings["SmtpAddress"]; }
		}

		public ConnectionStringSettings ReadModelDatabase
		{
			get { return ConfigurationManager.ConnectionStrings["ReadModelDatabase"]; }
		}

		public string AddUserFromAddress
		{
			get { return ConfigurationManager.AppSettings["AddUserFromAddress"]; }
		}

		public string AddUserToAddress
		{
			get { return ConfigurationManager.AppSettings["AddUserToAddress"]; }
		}

		public string AddUserToGroup
		{
			get { return ConfigurationManager.AppSettings["AddUserToGroup"]; }
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

		public bool IsInternalDataAccessible
		{
			get { return Convert.ToBoolean(ConfigurationManager.AppSettings["InternalDataAccessible"]); }
		}

		public string OverheadTimeProjectCode
		{
			get { return ConfigurationManager.AppSettings["OverheadTimeProjectCode"]; }
		}

		public string InvoiceTemplateLocation
		{
			get { return ConfigurationManager.AppSettings["InvoiceTemplateLocation"]; }
		}
		#endregion
	}
}