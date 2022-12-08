using System;

namespace Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers
{
    public static class ObjectExtension
    {
        public static T GetPropertyValue<T>(this object item, string propertyName)
        {
            var value = item.GetType().GetProperty(propertyName).GetValue(item, null);
            if (value is T)
            {
                return (T)value;
            }
            else
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }
    }
}
