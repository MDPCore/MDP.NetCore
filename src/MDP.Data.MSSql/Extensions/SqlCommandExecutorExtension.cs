using Dapper;
using Microsoft.Data.SqlClient;

namespace MDP.Data.MSSql
{
    public static partial class SqlClientExtension
    {
        // Methods
        public static T? ExecuteParse<T>(this SqlClient command, List<TypeHandler>? typeHandlerList = null)
        {
            #region Contracts

            if (command == null) throw new ArgumentException();

            #endregion

            // Return
            return command.ExecuteParseAll<T>(typeHandlerList).FirstOrDefault();
        }

        public static dynamic? ExecuteParse(this SqlClient command, List<TypeHandler>? typeHandlerList = null)
        {
            #region Contracts

            if (command == null) throw new ArgumentException();

            #endregion

            // Return
            return command.ExecuteParseAll(typeHandlerList).FirstOrDefault();
        }
    }

    public static partial class SqlClientExtension
    {
        // Methods
        public static List<T> ExecuteParseAll<T>(this SqlClient command, List<TypeHandler>? typeHandlerList = null)
        {
            #region Contracts

            if (command == null) throw new ArgumentException();

            #endregion

            // Require
            if (typeHandlerList == null) typeHandlerList = new List<TypeHandler>();

            // TypeHandler
            SqlMapper.ResetTypeHandlers();
            foreach (var typeHandler in typeHandlerList)
            {
                SqlMapper.AddTypeHandler(typeHandler.Type, typeHandler);
            }

            // Execute
            using (SqlDataReader reader = command.ExecuteReader())
            {
                // Parse
                var resultList = reader.Parse<T>();
                if (resultList == null) throw new InvalidOperationException("resultList=null");

                // Return
                return resultList.ToList();
            }
        }

        public static List<dynamic> ExecuteParseAll(this SqlClient command, List<TypeHandler>? typeHandlerList = null)
        {
            #region Contracts

            if (command == null) throw new ArgumentException();

            #endregion

            // Require
            if (typeHandlerList == null) typeHandlerList = new List<TypeHandler>();

            // TypeHandler
            SqlMapper.ResetTypeHandlers();
            foreach (var typeHandler in typeHandlerList)
            {
                SqlMapper.AddTypeHandler(typeHandler.Type, typeHandler);
            }

            // Execute
            using (SqlDataReader reader = command.ExecuteReader())
            {
                // Parse
                var resultList = reader.Parse();
                if (resultList == null) throw new InvalidOperationException("resultList=null");

                // Return
                return resultList.ToList();
            }
        }
    }
}
