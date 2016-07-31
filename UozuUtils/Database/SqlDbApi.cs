using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Uozu.Utils.Database
{
    public class SqlDbApi : IDbApi
    {
        private readonly string _connString;

        public SqlDbApi(string connectionString)
        {
            _connString = connectionString;
        }

        public int ExecuteNonQuery(IDbCommand cmd)
        {
            using (var con = CreateNewOpenConnection())
            {
                cmd.Connection = con;
                return cmd.ExecuteNonQuery();
            }
        }

        public int ExecuteNonQuery(string command)
        {
            using (var con = CreateNewOpenConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = command;
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public T ExecuteScalar<T>(string command)
        {
            return ExecuteScalarImpl<T>(command);
        }

        public T ExecuteScalar<T>(string command, params object[] sqlParams)
        {
            return ExecuteScalarImpl<T>(command, sqlParams);
        }

        private T ExecuteScalarImpl<T>(string command, params object[] sqlParams)
        {
            using (var con = CreateNewOpenConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = command;
                    if (sqlParams != null)
                    {
                        foreach (var p in sqlParams)
                            cmd.Parameters.Add(p);
                    }
                    var result = cmd.ExecuteScalar();
                    return (T) Convert.ChangeType(result, typeof(T));
                }
            }
        }

        public IEnumerable<T> ExecuteReader<T>(string query, Func<IDataRecord, T> dataObjectWriter)
        {
            return ExecuteReaderImpl(query, dataObjectWriter, CommandType.Text);
        }

        public IEnumerable<T> ExecuteReader<T>(string query, Func<IDataRecord, T> dataObjectWriter, CommandType commandType)
        {
            return ExecuteReaderImpl(query, dataObjectWriter, commandType);
        }

        public IEnumerable<T> ExecuteReader<T>(string query, Func<IDataRecord, T> dataObjectWriter,
            CommandType commandType, params object[] sqlParams)
        {
            return ExecuteReaderImpl(query, dataObjectWriter, commandType, sqlParams);
        }

        // cheers to Dapper for this (https://github.com/StackExchange/dapper-dot-net)
        private IEnumerable<T> ExecuteReaderImpl<T>(string query, Func<IDataRecord, T> dataObjectWriter,
            CommandType commandType, params object[] sqlParams)
        {
            using (var con = CreateNewOpenConnection())
            {
                IDbCommand cmd = null;
                IDataReader reader = null;
                try
                {
                    cmd = con.CreateCommand();
                    cmd.CommandText = query;
                    cmd.CommandType = commandType;
                    if (sqlParams != null)
                    {
                        foreach (var p in sqlParams)
                            cmd.Parameters.Add(p);
                    }
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        yield return dataObjectWriter(reader);
                    }
                    reader.Dispose();
                }
                finally
                {
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                        {
                            try { cmd.Cancel(); }
                            catch { } // don't overwrite existing exception
                        }
                        reader.Dispose();
                    }
                    cmd?.Dispose();
                }
            }
        }

        public T Insert<T>(T obj)
        {
            using (var con = CreateNewOpenConnection())
            {
                using (var cmd = SqlMapper.GetInsertCommand(obj))
                {
                    cmd.Connection = con;
                }
            }
            return obj;
        }

        private SqlConnection CreateNewOpenConnection()
        {
            var con = new SqlConnection(_connString);
            con.Open();
            return con;
        }
    }
}
