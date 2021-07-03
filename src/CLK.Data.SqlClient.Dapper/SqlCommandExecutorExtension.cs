using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CLK.Data.SqlClient.Dapper
{
    public static class SqlCommandEntityExtension
    {
        // Methods
        public static T ExecuteParse<T>(this SqlCommandEntity command, List<TypeHandler> typeHandlerList = null)
        {
            #region Contracts

            if (command == null) throw new ArgumentException();

            #endregion

            // Return
            return command.ExecuteParseAll<T>(typeHandlerList).FirstOrDefault();
        }

        public static dynamic ExecuteParse(this SqlCommandEntity command, List<TypeHandler> typeHandlerList = null)
        {
            #region Contracts

            if (command == null) throw new ArgumentException();

            #endregion

            // Return
            return command.ExecuteParseAll(typeHandlerList).FirstOrDefault();
        }


        public static List<T> ExecuteParseAll<T>(this SqlCommandEntity command, List<TypeHandler> typeHandlerList = null)
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

        public static List<dynamic> ExecuteParseAll(this SqlCommandEntity command, List<TypeHandler> typeHandlerList = null)
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
