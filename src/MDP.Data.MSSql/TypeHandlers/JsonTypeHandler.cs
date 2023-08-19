using System;
using System.Data;
using System.Text.Json;

namespace MDP.Data.MSSql
{
    public class JsonTypeHandler<T> : TypeHandler<T>
    {
        // Methods
        public override T Parse(object value)
        {
            // Deserialize
            var valueObject = JsonSerializer.Deserialize<T>((string)value);
            if (valueObject == null) throw new InvalidOperationException($"{nameof(valueObject)}=null");

            // Return
            return valueObject;
        }

        public override void SetValue(IDbDataParameter parameter, T value)
        {
            // Serialize
            if (value != null) parameter.Value = JsonSerializer.Serialize(value);
            if (value == null) parameter.Value = DBNull.Value;

            // Type
            parameter.DbType = DbType.String;
        }
    }
}
