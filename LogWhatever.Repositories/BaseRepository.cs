using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using LogWhatever.Common.CQRS;
using LogWhatever.Common.Configuration;
using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Caching;

namespace LogWhatever.Repositories
{
	public class BaseRepository
	{
		#region Properties
		public IConfigurationProvider ConfigurationProvider { get; set; }
		public ICollectionCache Cache { get; set; }
		public IMessageDispatcher Dispatcher { get; set; }
		#endregion

		#region Protected Methods
		protected internal virtual IEnumerable<TModelType> Retrieve<TModelType>(string query) where TModelType : BaseModel
		{
			return Cache.StoreOrRetrieve<TModelType>(Query<TModelType>(query));
		}
		#endregion

		#region Private Methods
		internal virtual IDbConnection CreateConnection(string connectionString)
		{
			return new SqlConnection(connectionString);
		}

		private IDbConnection OpenConnection(string connectionString = null)
		{
			if (connectionString == null)
				connectionString = ConfigurationProvider.ReadModelDatabase.ConnectionString;

			var connection = CreateConnection(connectionString);
			connection.Open();
			return connection;
		}

		private IEnumerable<TQueriedObjectType> Query<TQueriedObjectType>(string query, object parameters = null, string connectionString = null)
		{
			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException("query");

			using (var connection = OpenConnection(connectionString))
			{
				return connection.Query<TQueriedObjectType>(query, parameters);
			}
		}
		#endregion
	}
}