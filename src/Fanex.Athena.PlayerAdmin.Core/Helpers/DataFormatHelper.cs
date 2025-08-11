namespace Fanex.Athena.PlayerAdmin.Core.Helpers
{
    using System;
    using System.Globalization;

    public static class DataFormatHelper
    {
        public const string DECIMALFORMAT20 = "{0:#,##0.00}";

        public static decimal AsDecimal(object value)
        {
            return AsDecimal(value, 0, CultureInfo.CurrentCulture);
        }

        public static decimal AsDecimal(object value, decimal defaultValue, IFormatProvider provider)
        {
            if (value == null || value == DBNull.Value)
            {
                return defaultValue;
            }

            decimal retval;

            try
            {
                retval = Convert.ToDecimal(value, provider ?? CultureInfo.CurrentCulture);
            }
            catch (InvalidCastException)
            {
                retval = defaultValue;
            }
            catch (FormatException)
            {
                retval = defaultValue;
            }

            return retval;
        }
    }
}