using System;
using System.Linq;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using Dapper;

namespace LogWhatever.Common.Extensions
{
    public static class DbConnectionExtensions
    {
        #region Public Methods
        public static IEnumerable<TMappedType> QueryPaged<TMappedType>(this IDbConnection connection, string query, int pageSize, int pageNumber, object queryParameters = null)
        {
            if (string.IsNullOrEmpty(query))
                throw new ArgumentNullException("query");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException("pageSize");
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException("pageNumber");

            return connection.Query<TMappedType>(CreatePagedQuery(query, pageSize, pageNumber, IsSQLCompactEdition(connection)), queryParameters);
        }

        public static void Insert(this IDbConnection connection, string table, object parameters, IEnumerable<string> ignore = null)
        {
	        if (string.IsNullOrEmpty(table))
		        throw new ArgumentNullException("table");
	        if (parameters == null)
		        throw new ArgumentNullException("parameters");
			if (ignore == null)
				ignore = new List<string>();

            connection.Execute(CreateInsertStatement(table, parameters, ignore), parameters);
        }

        public static void Update(this IDbConnection connection, string table, object parameters, params string[] where)
        {
            if (string.IsNullOrEmpty(table))
                throw new ArgumentNullException("table");
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (where == null || where.Length < 1)
                throw new ArgumentNullException("where");

            connection.Execute(CreateUpdateStatement(table, where, parameters), parameters);
        }

        public static int Count(this IDbConnection connection, string table, object filter = null, bool inexact = false, bool and = true)
        {
            if (string.IsNullOrEmpty(table))
                throw new ArgumentNullException("table");

	        return connection.Query<int>(CreateSelectCountQuery(table, filter, inexact, and), filter).First();
        }
	    #endregion

        #region Private Methods
        internal static bool IsSQLCompactEdition(IDbConnection connection)
        {
            return connection.Database.EndsWith(".sdf");
        }

        internal static string CreatePagedQuery(string query, int pageSize, int pageNumber, bool generateSQLCEStylePagedQuery = false)
        {
	        return generateSQLCEStylePagedQuery ? CreateSQLCEPagedQuery(query, pageSize, pageNumber) : CreateSQLServerPagedQuery(query, pageSize, pageNumber);
        }

	    internal static string CreateSQLServerPagedQuery(string query, int pageSize, int pageNumber)
        {
            var table = GetTableName(query);
            var where = DeriveWhereClause(query, table);
            var order = DeriveOrder(query);
            var innerQuery = string.Format("select tbl.*, row_number() over (order by {0}) rownum from {1} as tbl{2}", order, table, where);
            var outerQuery = string.Format("select * from ({0}) as seq where seq.rownum between {1} and {2} order by seq.rownum", innerQuery, ((pageNumber - 1) * pageSize) + 1, pageNumber * pageSize);
            return outerQuery;
        }

        internal static string CreateSQLCEPagedQuery(string query, int pageSize, int pageNumber)
        {
            return String.Format("{0} OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", query, (pageSize * (pageNumber - 1)), pageSize);
        }

        internal static string DeriveOrder(string query)
        {
            var order = "newid()";
            if (query.Contains("order by"))
                order = query.Substring(query.IndexOf("order by", StringComparison.Ordinal) + 8).Trim();
            return order;
        }

        internal static string DeriveWhereClause(string query, string table)
        {
            var where = "";
            if (query.Contains("where"))
            {
                where = query.Substring(query.IndexOf(table, StringComparison.Ordinal) + table.Length);
                if (where.Contains("order"))
                    where = where.Substring(0, where.IndexOf("order", StringComparison.Ordinal) - 1);
            }
            return where;
        }

        internal static string GetTableName(string query)
        {
            if (query.Contains("where"))
                return query.Between("from", "where", StringComparison.OrdinalIgnoreCase).Trim();
            if (query.Contains("order"))
                return query.Between("from", "order", StringComparison.OrdinalIgnoreCase).Trim();
            return query.Substring(query.IndexOf("from", StringComparison.OrdinalIgnoreCase) + 4).Trim();
        }

        internal static string CreateInsertStatement(string table, object parameters, IEnumerable<string> ignore = null)
        {
			if (ignore == null)
				ignore = new string[] {};
            if (parameters == null)
                return "";

	        var properties = parameters.GetType().GetProperties();
			if (properties.Length == 0 || properties.Count(x => !ignore.Contains(x.Name)) == 0)
				return "";

	        var query = string.Format("insert into {0} (", table);
            var columns = "";
            var variables = "";

            properties.Where(x => !ignore.Contains(x.Name)).Each(x =>
            {
                columns += String.Format(", [{0}]", x.Name);
                variables += ", @" + x.Name;
            });

            return string.Format("{0}{1}) values ({2})", query, columns.Substring(2), variables.Substring(2));
        }

        internal static string CreateUpdateStatement(string table, IEnumerable<string> where, object parameters)
        {
            var query = String.Format("update {0} set ", table);
            foreach (var property in TypeDescriptor.GetProperties(parameters).Cast<PropertyDescriptor>().Select(x => x.Name).Where(x => !where.Contains(x)))
                query += String.Format("[{0}] = @{0}, ", property);
            query = query.Substring(0, query.Length - 2) + " where ";
            foreach (var filter in where)
                query += String.Format("[{0}] = @{0} and ", filter);
            return query.Substring(0, query.Length - 5);
        }

		internal static string CreateSelectCountQuery(string table, object filter, bool inexact, bool and)
		{
			var query = "select count(*) from " + table;
			if (filter != null)
				query += CreateWhereClauseFromParametersObject(filter, inexact, and);
			return query;
		}

		internal static string CreateWhereClauseFromParametersObject(object filter, bool inexact, bool and)
		{
			var properties = TypeDescriptor.GetProperties(filter).Cast<PropertyDescriptor>().Where(x => x.GetValue(filter) != null && !x.GetValue(filter).Equals(x.PropertyType == typeof(Guid) ? Guid.Empty : x.PropertyType.IsValueType ? Activator.CreateInstance(x.PropertyType) : null)).ToArray();

			var where = " where ";
			properties.Each(x => where += String.Format((inexact ? "[{0}] like('!' + @{0} + '%') escape '!'" : "[{0}] = @{0}") + (and ? " and " : " or "), x.Name));
			return where.Substring(0, where.Length - (and ? 5 : 4));
		}
        #endregion
    }
}