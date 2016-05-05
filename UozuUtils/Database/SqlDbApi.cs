using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Uozu.Utils.Database
{
    public class SqlDbApi
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

        // cheers to Dapper for this (https://github.com/StackExchange/dapper-dot-net)
        public IEnumerable<T> ExecuteReader<T>(string query, Func<IDataReader, T> dataObjectWriter)
        {
            using (var con = CreateNewOpenConnection())
            {
                IDbCommand cmd = null;
                IDataReader reader = null;
                try
                {
                    cmd = con.CreateCommand();
                    cmd.CommandText = query;
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
                            try   { cmd.Cancel(); }
                            catch { /* don't spoil the existing exception */ }
                        }
                        reader.Dispose();
                    }
                    cmd?.Dispose();
                }
            }
        }

        private SqlConnection CreateNewOpenConnection()
        {
            var con = new SqlConnection(_connString);
            con.Open();
            return con;
        }
    }
}
