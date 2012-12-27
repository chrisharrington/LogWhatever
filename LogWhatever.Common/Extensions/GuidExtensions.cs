using System;

namespace LogWhatever.Common.Extensions
{
	public static class GuidExtensions
	{
		#region Public Methods
		public static Guid? ParseWithoutFail(string value)
		{
			if (string.IsNullOrEmpty(value))
				return null;

			Guid result;
			if (!Guid.TryParse(value, out result))
				return null;
			return result;
		}
		#endregion
	}
}