using System;
using System.Linq;
using System.Text;

namespace LogWhatever.Common.Extensions
{
    public static class StringExtensions
    {
        #region Public Methods
        public static string Between(this string value, string first, string second, StringComparison comparison = StringComparison.CurrentCulture)
        {
            var begin = string.IsNullOrEmpty(first) || value.IndexOf(first, comparison) == -1 ? 0 : value.IndexOf(first, comparison) + first.Length;
            var end = string.IsNullOrEmpty(second) || value.IndexOf(second, comparison) == -1 ? value.Length : value.IndexOf(second, comparison);

            if (begin > end)
                return value;

            return value.Substring(begin, end - begin);
        }

        public static string FromUpperCamelCaseToSpaced(this string value)
        {
            var sb = new StringBuilder();
            foreach (var c in value)
            {
                if (Char.IsUpper(c))
                    sb.Append(" ");
                sb.Append(c);
            }
            var workingString = sb.ToString().Trim();
			return workingString.Split(' ').Any(x => x.Length == 1) ? value : workingString;
        }

		public static string Capitalize(this string value)
		{
			if (string.IsNullOrEmpty(value))
				return "";
			if (value.Length == 1)
				return value.ToUpper();
			return value[0].ToString().ToUpper() + value.Substring(1);
		}
        #endregion
    }
}