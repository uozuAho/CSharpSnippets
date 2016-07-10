using System;
using System.Collections.Generic;
using System.Data;

namespace Uozu.Utils.Database
{
    public interface IDbApi
    {
        int ExecuteNonQuery(IDbCommand cmd);
        int ExecuteNonQuery(string command);
        IEnumerable<T> ExecuteReader<T>(string query, Func<IDataRecord, T> dataObjectWriter);
        IEnumerable<T> ExecuteReader<T>(string query, Func<IDataRecord, T> dataObjectWriter, CommandType commandType);
        IEnumerable<T> ExecuteReader<T>(string query, Func<IDataRecord, T> dataObjectWriter, CommandType commandType, params object[] sqlParams);
        T Insert<T>(T obj);
    }
}
