using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LogWhatever.Common.Extensions
{
	public static class ObjectExtensions
	{
		#region Public Methods
		public static bool PropertiesEqual(this object first, object second)
		{
			if (first == null || second == null)
				return false;

			var secondProperties = second.GetType().GetProperties().ToDictionary(x => x.Name);

			foreach (var property in first.GetType().GetProperties().Where(x => secondProperties.ContainsKey(x.Name)))
			{
				var firstValue = property.GetValue(first, null);
				var secondValue = secondProperties[property.Name].GetValue(second, null);
				if ((firstValue == null && secondValue != null) || (firstValue != null && secondValue == null) || (firstValue != null && !firstValue.Equals(secondValue)))
					return false;
			}

			return true;
		}

		public static bool IsValid(this object obj)
		{
			foreach (var property in TypeDescriptor.GetProperties(obj.GetType()).Cast<PropertyDescriptor>())
			{
				var isValid = property.Attributes.OfType<ValidationAttribute>().All(x => x.IsValid(property.GetValue(obj)));
				if (!isValid)
					return false;
			}

			return true;
		}
		#endregion
	}
}