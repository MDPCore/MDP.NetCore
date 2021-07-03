using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CLK.Data.SqlClient.Dapper
{
    public interface TypeHandler : SqlMapper.ITypeHandler
    {
        // Properties
        Type Type { get; }
    }

    public abstract class TypeHandler<T> : SqlMapper.TypeHandler<T>, TypeHandler
    {
        // Properties
        public Type Type { get { return typeof(T); } }
    }
}
