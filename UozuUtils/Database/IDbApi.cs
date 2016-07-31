using System;
using System.Collections.Generic;
using System.Data;

namespace Uozu.Utils.Database
{
    public interface IDbApi
    {
        int ExecuteNonQuery(IDbCommand cmd);
        int ExecuteNonQuery(string cmd);
        T ExecuteScalar<T>(string cmd);
        T ExecuteScalar<T>(string cmd, params object[] sqlParams);
        IEnumerable<T> ExecuteReader<T>(string query, Func<IDataRecord, T> dataObjectWriter);
        IEnumerable<T> ExecuteReader<T>(string query, Func<IDataRecord, T> dataObjectWriter, CommandType commandType);
        IEnumerable<T> ExecuteReader<T>(string query, Func<IDataRecord, T> dataObjectWriter, CommandType commandType, params object[] sqlParams);
    }
}
