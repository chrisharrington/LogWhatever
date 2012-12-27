using System;

namespace LogWhatever.Common.Extensions
{
	public static class DateTimeExtensions
	{
		#region Public Methods
		public static bool IsApproximatelyEqualTo(this DateTime first, DateTime second, int seconds = 5)
		{
			return second.AddSeconds(-1 * seconds) < first && second.AddSeconds(seconds) > first;
		}

		public static DateTime BeginningOfTheWeek(this DateTime value)
		{
			var date = new DateTime(value.Year, value.Month, value.Day);
			while (date.DayOfWeek != DayOfWeek.Sunday)
				date = date.AddDays(-1);
			return date;
		}
		#endregion
	}
}