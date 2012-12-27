using System.Collections.Generic;

namespace LogWhatever.Common.Extensions
{
	public static class DecimalExtensions
	{
		#region Public Methods
		public static IEnumerable<decimal> DivideIntoWholeNumbers(this decimal value, int divisor)
		{
			if (divisor == 0 || value == 0)
				return new List<decimal>();

			var result = new decimal[divisor];
			var index = 0;
			while (value >= 0)
			{
				result[index++] += value > 1 ? 1 : (1 - value);
				value--;
				if (index >= result.Length)
					index = 0;
			}
			return result;
		}
		#endregion
	}
}