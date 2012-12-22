using System.Configuration;

namespace LogWhatever.Common.Configuration
{
	public interface IConfigurationProvider
	{
		#region Properties
		ConnectionStringSettings ReadModelDatabase { get; }
		ConnectionStringSettings BlobStorage { get; }

		string FogbugzUserName { get; }
		string FogbugzPassword { get; }
		string FogbugzAPILocation { get; }
		string SmtpAddress { get; }
		string AddUserFromAddress { get; }
		string AddUserToAddress { get; }
		string AddUserToGroup { get; }
		string FromEmailAddress { get; }
		string SendGridUserName { get; }
		string SendGridPassword { get; }
		bool IsInternalDataAccessible { get; }
		string OverheadTimeProjectCode { get; }
		string InvoiceTemplateLocation { get; }
		#endregion
	}
}