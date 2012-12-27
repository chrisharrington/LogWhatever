using System;
using System.Linq;
using System.ComponentModel;

namespace LogWhatever.Common.Extensions
{
    public static class MappingExtensions
    {
        #region Public Methods
        public static TMappedType Map<TMappedType>(this object obj) where TMappedType : new()
        {
	        return MapToType(obj, Activator.CreateInstance<TMappedType>());
        }

	    public static object Map(this object obj, Type toMapTo) 
        {
		    return MapToType(obj, Activator.CreateInstance(toMapTo));
        }
        #endregion

        #region Private Methods
		internal static bool ConvertBooleanStringValue(string value)
        {
            value = value.ToLower();
            if (value == "yes")
                return true;
            if (value == "no")
                return false;

            bool result;
            if (!bool.TryParse(value, out result))
                return false;
            return result;
        }

		internal static TMappedType MapToType<TMappedType>(object obj, TMappedType destination) where TMappedType : new()
		{
			var destinationProperties = TypeDescriptor.GetProperties(destination).Cast<PropertyDescriptor>();
			foreach (var sourceProperty in TypeDescriptor.GetProperties(obj).Cast<PropertyDescriptor>())
			{
				var destinationProperty = destinationProperties.FirstOrDefault(x => x.Name == sourceProperty.Name);
				if (destinationProperty == null || sourceProperty.GetValue(obj) == null)
					continue;

				SetPropertyValue(obj, destination, sourceProperty, destinationProperty);
			}
			return destination;
		}

	    internal static void SetPropertyValue<TMappedType>(object obj, TMappedType destination, PropertyDescriptor sourceProperty, PropertyDescriptor destinationProperty) where TMappedType : new()
	    {
		    if (destinationProperty.PropertyType == typeof (bool))
			    destinationProperty.SetValue(destination, ConvertBooleanStringValue(sourceProperty.GetValue(obj).ToString()));
			else if (destinationProperty.PropertyType != sourceProperty.PropertyType && destinationProperty.PropertyType == typeof(Guid))
			    destinationProperty.SetValue(destination, Guid.Parse(sourceProperty.GetValue(obj).ToString()));
		    else if (destinationProperty.PropertyType != sourceProperty.PropertyType)
			    destinationProperty.SetValue(destination, Convert.ChangeType(sourceProperty.GetValue(obj), destinationProperty.PropertyType));
		    else
			    destinationProperty.SetValue(destination, sourceProperty.GetValue(obj));
	    }
	    #endregion
    }
}