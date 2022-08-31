using Dapper;

namespace MDP.Data.SqlClient
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
