using System.Configuration;

namespace LogWhatever.Common.Configuration
{
	public interface IConfigurationProvider
	{
		#region Properties
		ConnectionStringSettings BlobStorage { get; }
		ConnectionStringSettings ReadModelDatabase { get; }

		string DataServiceLocation { get; }
		string FromEmailAddress { get; }
		string SendGridUserName { get; }
		string SendGridPassword { get; }
		#endregion
	}
}