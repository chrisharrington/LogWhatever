using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using LogWhatever.Common.Configuration;

namespace LogWhatever.Repositories
{
	public class BaseRepository
	{
		#region Properties
		public IConfigurationProvider ConfigurationProvider { get; set; }
		#endregion

		#region Protected Methods
		protected internal virtual IDbConnection OpenConnection(string connectionString = null)
		{
			if (connectionString == null)
				connectionString = ConfigurationProvider.ReadModelDatabase.ConnectionString;

			var connection = CreateConnection(connectionString);
			connection.Open();
			return connection;
		}

		protected internal virtual IEnumerable<TQueriedObjectType> Query<TQueriedObjectType>(string query, object parameters = null, string connectionString = null)
		{
			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException("query");

			using (var connection = OpenConnection(connectionString))
			{
				return connection.Query<TQueriedObjectType>(query, parameters);
			}
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