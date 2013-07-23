using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
		protected internal virtual IEnumerable<TModelType> Retrieve<TModelType>(string query, Func<TModelType, bool> filter = null) where TModelType : BaseModel
		{
			var results = Query<TModelType>(query);
			SetDatesAsUTC(results);
			Cache.StoreOrRetrieve<TModelType>(results);
			return filter == null ? results : results.Where(filter);
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

		private void SetDatesAsUTC(IEnumerable<object> items)
		{
			foreach (var item in items)
				foreach (var property in item.GetType().GetProperties().Where(x => x.PropertyType == typeof(DateTime)))
					property.SetValue(item, DateTime.SpecifyKind((DateTime) property.GetValue(item), DateTimeKind.Utc));
		}
		#endregion
	}
}