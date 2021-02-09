using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CLK.Data.SqlClient.Dapper
{
    public class JsonTypeHandler<T> : TypeHandler<T>
    {
        // Methods
        public override T Parse(object value)
        {
            // DeserializeObject
            return JsonConvert.DeserializeObject<T>((string)value);
        }

        public override void SetValue(IDbDataParameter parameter, T value)
        {
            // SerializeObject
            if (value != null) parameter.Value = JsonConvert.SerializeObject(value);
            if (value == null) parameter.Value = DBNull.Value;

            // Type
            parameter.DbType = DbType.String;
        }
    }
}
