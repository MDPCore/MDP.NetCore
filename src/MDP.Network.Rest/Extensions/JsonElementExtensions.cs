using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.Network.Rest
{
    public static class JsonElementExtensions
    {
        // Constants
        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };


        // Methods
        public static TResult? GetProperty<TResult>(this JsonElement jsonElement, string propertyName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(propertyName) == true) throw new ArgumentException($"{nameof(propertyName)}=null");

            #endregion

            // Variables
            var resultType = typeof(TResult);
            var defaultValue = typeof(TResult).IsValueType == true ? default(TResult) : default(TResult?);

            // ResultElement
            if (jsonElement.TryGetProperty(propertyName, out var resultElement) == false) return defaultValue;
            if (resultElement.GetType() == resultType) return (TResult?)(object)resultElement;

            // Primitive
            if (resultType.IsPrimitive == true || (resultType.IsPrimitive == false && Nullable.GetUnderlyingType(typeof(TResult))?.IsPrimitive == true))
            {
                // Parse
                if (resultType == typeof(bool) || resultType == typeof(bool?)) { return (TResult?)(object)resultElement.GetBoolean(); }
                if (resultType == typeof(byte) || resultType == typeof(byte?)) { if (resultElement.TryGetByte(out var result) == true) return (TResult?)(object)result; else return defaultValue; }
                if (resultType == typeof(short) || resultType == typeof(short?)) { if (resultElement.TryGetInt16(out var result) == true) return (TResult?)(object)result; else return defaultValue; }
                if (resultType == typeof(int) || resultType == typeof(int?)) { if (resultElement.TryGetInt32(out var result) == true) return (TResult?)(object)result; else return defaultValue; }
                if (resultType == typeof(long) || resultType == typeof(long?)) { if (resultElement.TryGetInt64(out var result) == true) return (TResult?)(object)result; else return defaultValue; }
                if (resultType == typeof(float) || resultType == typeof(float?)) { if (resultElement.TryGetSingle(out var result) == true) return (TResult?)(object)result; else return defaultValue; }
                if (resultType == typeof(double) || resultType == typeof(double?)) { if (resultElement.TryGetDouble(out var result) == true) return (TResult?)(object)result; else return defaultValue; }
                if (resultType == typeof(decimal) || resultType == typeof(decimal?)) { if (resultElement.TryGetDecimal(out var result) == true) return (TResult?)(object)result; else return defaultValue; }
               
                // Throw
                throw new InvalidOperationException($"ResultType={typeof(TResult)}, ResultValue={resultElement.GetString()}");
            }

            // String
            if (resultType == typeof(string))
            {
                // Parse
                var result = resultElement.GetString();
                if (result == null) return defaultValue;
                if (result != null) return (TResult?)(object)result;
            }

            // DateTime
            if (resultType == typeof(DateTime))
            {
                // Parse
                if (resultElement.TryGetDateTime(out var result) == true)
                {
                    return (TResult?)(object)result;
                }
                return defaultValue;
            }

            // Class
            if (resultType.IsClass == true && resultType.IsAbstract == false && resultType.IsInterface == false)
            {
                // Parse
                var result = resultElement.Deserialize<TResult>(_serializerOptions);
                if (result == null) return defaultValue;
                if (result != null) return (TResult?)(object)result;
            }

            // Throw
            throw new InvalidOperationException($"ResultType={typeof(TResult)}, ResultValue={resultElement.GetRawText()}");
        }
    }
}
