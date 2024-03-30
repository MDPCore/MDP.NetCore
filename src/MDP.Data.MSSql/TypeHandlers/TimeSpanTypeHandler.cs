using System;
using System.Data;

namespace MDP.Data.MSSql
{
    public class TimeSpanTypeHandler : TypeHandler<TimeSpan>
    {
        // Methods
        public override TimeSpan Parse(object value)
        {
            // Require
            if (value == null) return TimeSpan.Zero;

            // Return
            return TimeSpan.FromTicks((long)value);
        }

        public override void SetValue(IDbDataParameter parameter, TimeSpan value)
        {
            // Value
            parameter.Value = value.Ticks;

            // Type
            parameter.DbType = DbType.Int64;
        }
    }
}
