using System;
using System.Data;
using System.Diagnostics;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Extensions
{
    public static class DataReaderExtension
    {
        [DebuggerStepThrough]
        public static decimal ToDecimal(this IDataReader reader, string name) => Converter.ToDecimal(reader[name]);

        [DebuggerStepThrough]
        public static bool ToBool(this IDataReader reader, string name) => Converter.ToBoolean(reader[name]);

        [DebuggerStepThrough]
        public static string ToString(this IDataReader reader, string name) => Converter.ToString(reader[name]);

        [DebuggerStepThrough]
        public static int ToInt(this IDataReader reader, string name) => Converter.ToInt(reader[name]);

        [DebuggerStepThrough]
        public static DateTime ToDateTime(this IDataReader reader, string name) => Converter.ToDateTime(reader[name]);
    }
}