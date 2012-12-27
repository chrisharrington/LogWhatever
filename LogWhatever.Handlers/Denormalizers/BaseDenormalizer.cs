using System.Data;
using System.Data.SqlClient;
using LogWhatever.Common.Configuration;

namespace LogWhatever.Handlers.Denormalizers
{
    public class BaseDenormalizer
	{
		#region Properties
		public IConfigurationProvider ConfigurationProvider { get; set; }
		#endregion

		#region Protected Methods
		internal protected virtual IDbConnection OpenConnection(string connectionString = null)
		{
			if (connectionString == null)
				connectionString = ConfigurationProvider.ReadModelDatabase.ConnectionString;

			var connection = CreateConnection(connectionString);
			connection.Open();
			return connection;
		}
		#endregion

		#region Private Methods
		internal virtual IDbConnection CreateConnection(string connectionString)
		{
			return new SqlConnection(connectionString);
		}
		#endregion
	}
}
