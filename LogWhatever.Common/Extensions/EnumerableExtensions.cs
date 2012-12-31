using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LogWhatever.Common.Extensions
{
    public static class EnumerableExtensions
    {
        #region Public Methods
        public static int Index<T>(this IEnumerable<T> list, Func<T, bool> func)
        {
	        if (func == null)
		        throw new ArgumentNullException("func");

            var count = 0;
            foreach (var item in list)
            {
                if (func(item))
                    return count;
                count++;
            }

            return -1;
        }

        public static IEnumerable<T> Each<T>(this IEnumerable<T> list, Action<T> func)
        {
	        if (func == null)
		        throw new ArgumentNullException("func");

            foreach (var item in list)
                func(item);
            return list;
        }

		public static IEnumerable<T> Add<T>(this IEnumerable<T> list, T value)
		{
			return new List<T>(list) {value};
		}

		public static IEnumerable<T> AddRange<T>(this IEnumerable<T> first, IEnumerable<T> second)
		{
			if (second == null || !second.Any())
				return first;

			first = first.Concat(second);
			return first;
		}

		public static IEnumerable<T> OrderByString<T>(this IEnumerable<T> list, string propertyName, bool isDescending = false)
		{
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			var param = Expression.Parameter(typeof(T), "item");

			var sortExpression = Expression.Lambda<Func<T, object>>
			(Expression.Convert(Expression.Property(param, propertyName), typeof(object)), param);

			return !isDescending ? list.AsQueryable().OrderBy(sortExpression) : list.AsQueryable().OrderByDescending(sortExpression);
		}

		public static IEnumerable<T> Distinct<T>(this IEnumerable<T> list, Func<T, T, bool> equalityComparer)
		{
			var results = new List<T>();
			foreach (var item in list.Where(item => !results.Aggregate(false, (current, result) => current || equalityComparer(item, result))))
				results.Add(item);
			return results;
		}  
        #endregion
    }
}