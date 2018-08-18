using System;
using System.Data.Common;

namespace Weekl.Core.Entities
{
    public abstract class BaseModel
    {
        public dynamic Get<T>(DbDataReader r, string columnName)
        {
            if (typeof(T) == typeof(byte))
                return (T)r[columnName];

            if (typeof(T) == typeof(short))
                return (T)r[columnName];

            if (typeof(T) == typeof(short?))
                return r[columnName] != DBNull.Value ? Convert.ToInt16(r[columnName]) : new short?();

            if (typeof(T) == typeof(int))
                return (int)r[columnName];

            if (typeof(T) == typeof(int?))
                return r[columnName] != DBNull.Value ? Convert.ToInt32(r[columnName]) : new int?();

            //if (typeof(T) == typeof(uint))
            //    return (uint)r[columnName];

            //if (typeof(T) == typeof(uint?))
            //    return r[columnName] != DBNull.Value ? Convert.ToUInt32(r[columnName]) : new uint?();

            if (typeof(T) == typeof(long))
                return (long)r[columnName];

            if (typeof(T) == typeof(long?))
                return r[columnName] != DBNull.Value ? Convert.ToInt64(r[columnName]) : new long?();

            if (typeof(T) == typeof(double))
                return Convert.ToDouble(r[columnName]);

            if (typeof(T) == typeof(double?))
                return r[columnName] != DBNull.Value ? Convert.ToDouble(r[columnName]) : new double?();

            if (typeof(T) == typeof(float))
                return Convert.ToSingle(r[columnName]);

            if (typeof(T) == typeof(float?))
                return r[columnName] != DBNull.Value ? Convert.ToSingle(r[columnName]) : new float?();

            if (typeof(T) == typeof(string))
                return r[columnName] != DBNull.Value ? r[columnName] : null;

            if (typeof(T) == typeof(DateTime))
                return (DateTime)r[columnName];

            if (typeof(T) == typeof(DateTime?))
                return r[columnName] != DBNull.Value ? (DateTime)r[columnName] : new DateTime?();

            if (typeof(T) == typeof(TimeSpan))
                return (TimeSpan)r[columnName];

            if (typeof(T) == typeof(TimeSpan?))
                return r[columnName] != DBNull.Value ? (TimeSpan)r[columnName] : new TimeSpan?();

            if (typeof(T) == typeof(bool))
                return Convert.ToBoolean(r[columnName]);

            if (typeof(T) == typeof(bool?))
                return r[columnName] != DBNull.Value ? Convert.ToBoolean(r[columnName]) : new bool?();

            if (typeof(T) == typeof(decimal))
                return Convert.ToDecimal(r[columnName]);

            if (typeof(T) == typeof(decimal?))
                return r[columnName] != DBNull.Value ? Convert.ToDecimal(r[columnName]) : new decimal?();

            if (typeof(T) == typeof(TimeSpan))
                return (TimeSpan)r[columnName];

            if (typeof(T) == typeof(TimeSpan?))
                return r[columnName] != DBNull.Value ? (TimeSpan)r[columnName] : new TimeSpan?();

            throw new Exception($"Type [{typeof(T).FullName}] is not supported.");
        }
    }
}
