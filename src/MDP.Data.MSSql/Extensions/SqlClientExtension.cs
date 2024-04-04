using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDP.Data.MSSql
{
    public static partial class SqlClientExtension
    {
        // Methods
        public static T ExecuteParse<T>(this SqlClient sqlClient, List<TypeHandler> typeHandlerList = null)
        {
            #region Contracts

            if (sqlClient == null) throw new ArgumentException();

            #endregion

            // Return
            return sqlClient.ExecuteParseAll<T>(typeHandlerList).FirstOrDefault();
        }

        public static dynamic ExecuteParse(this SqlClient sqlClient, List<TypeHandler> typeHandlerList = null)
        {
            #region Contracts

            if (sqlClient == null) throw new ArgumentException();

            #endregion

            // Return
            return sqlClient.ExecuteParseAll(typeHandlerList).FirstOrDefault();
        }
    }

    public static partial class SqlClientExtension
    {
        // Methods
        public static List<T> ExecuteParseAll<T>(this SqlClient sqlClient, List<TypeHandler> typeHandlerList = null)
        {
            #region Contracts

            if (sqlClient == null) throw new ArgumentException();

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
            using (SqlDataReader reader = sqlClient.ExecuteReader())
            {
                // Parse
                var resultList = reader.Parse<T>();
                if (resultList == null) throw new InvalidOperationException("resultList=null");

                // Return
                return resultList.ToList();
            }
        }

        public static List<dynamic> ExecuteParseAll(this SqlClient sqlClient, List<TypeHandler> typeHandlerList = null)
        {
            #region Contracts

            if (sqlClient == null) throw new ArgumentException();

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
            using (SqlDataReader reader = sqlClient.ExecuteReader())
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
