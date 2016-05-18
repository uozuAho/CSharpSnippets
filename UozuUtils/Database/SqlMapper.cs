using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;

namespace Uozu.Utils.Database
{
    public class SqlMapper
    {
        private static ConcurrentDictionary<Type, Delegate> ExpressionCache =
                       new ConcurrentDictionary<Type, Delegate>();

        private static readonly Dictionary<Type, string> _idTypeMap = new Dictionary<Type, string>
        {
            { typeof(int), "int" },
            { typeof(long), "bigint" },
            { typeof(Guid), "uniqueidentifier" }
        };

        /// <summary>
        /// Read a record into the given data type.
        /// </summary>
        /// <remarks>
        /// Uses reflection, however compiles the reflection code on first run and 
        /// stores the compiled code in a cache. The result is that this reader 
        /// is 4-5x faster than using reflection for every read.
        /// 
        /// If you're getting cryptic error messages from this method, use 
        /// MapObjectWithReflection to debug.
        /// </remarks>
        public static T MapObjectWithCachedReflection<T>(IDataRecord reader) where T : new()
        {
            var readRow = GetMapper<T>();
            return readRow(reader);
        }

        /// <summary>
        /// Read a record into the given data type, using reflection to get the field names.
        /// </summary>
        /// <remarks>
        /// Is about 4-5x as slow as a hard-coded reader.
        /// </remarks>
        // FIXME: Incorrectly converts nullables to default values if the given type
        //        doesn't correctly match the reader. See TryToMapNullableToNonNullable_Reflection test
        public static T MapObjectWithReflection<T>(IDataRecord reader) where T : new()
        {
            var t = new T();
            foreach (var property in typeof(T).GetProperties())
            {
                var val = reader[property.Name];
                var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                var cval = (val == null || val == DBNull.Value) ? null : Convert.ChangeType(val, type);
                property.SetValue(t, cval);
            }
            return t;
        }

        public static SqlCommand GetInsertCommand<T>(T obj)
        {
            return GetInsertCommandInternal(obj, null);
        }

        public static SqlCommand GetInsertCommand<T>(T obj, string tableName)
        {
            return GetInsertCommandInternal(obj, tableName);
        }

        public static SqlCommand GetInsertCommand<T>(T obj, string tableName, string idCol = "id")
        {
            return GetInsertCommandInternal(obj, tableName);
        }

        public static SqlCommand GetInsertAndReturnIdCommand<T>(T obj, Type idType, string idCol = "id")
        {
            return GetInsertCommandInternal(obj, null, idCol, idType);
        }

        // TODO: work out the id type from idCol
        public static SqlCommand GetInsertAndReturnIdCommand<T>(T obj, string tableName, string idCol, Type idType)
        {
            return null;
            //return GetInsertCommandInternal(obj, tableName, true, idCol, idType);
        }

        private static SqlCommand GetInsertCommandInternal<T>(T obj, string tableName, string idCol = null, Type idType = null)
        {
            var cmd = new SqlCommand();
            if (tableName == null)
                tableName = typeof(T).Name;
            cmd.CommandText = GetInsertCmdString(obj, tableName, idCol, idType);
            AddParameters(cmd, obj);
            return cmd;
        }

        private static string GetInsertCmdString<T>(T obj, string tableName, string idCol, Type idType)
        {
            bool returnInsertedId = idCol != null && idType != null;
            var colList = new StringBuilder("(");
            var paramList = new StringBuilder("(");
            foreach (var property in typeof(T).GetProperties())
            {
                var name = property.Name;
                var type = property.PropertyType;
                colList.Append(name).Append(",");
                paramList.Append("@").Append(name).Append(",");
            }
            colList.Remove(colList.Length - 1, 1).Append(")");
            paramList.Remove(paramList.Length - 1, 1).Append(")");
            var cmd = new StringBuilder();
            if (returnInsertedId)
            {
                // TODO: better name for temp table that won't get overwritten by parameter values?
                cmd.Append("declare @xxx table (id ");
                // TODO: test unknown types 
                cmd.Append(_idTypeMap[idType]);
                cmd.Append("); ");
            }
            cmd.Append("insert into [").Append(tableName).Append("] ")
                .Append(colList).Append(" values ").Append(paramList).Append(";");
            if (returnInsertedId)
                cmd.Append(" select id from @xxx;");
            return cmd.ToString();
        }

        public static void AddParameters<T>(SqlCommand cmd, T obj)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                cmd.Parameters.AddWithValue("@" + property.Name, property.GetValue(obj));
            }
        }

        /// <summary>
        /// Returns a mapper that reads record into the given data type.
        /// </summary>
        /// <remarks>
        /// Largely inspired by 
        /// http://www.codeproject.com/Articles/503527/Reflection-optimization-techniques
        /// Quick testing shows that this performs at similar speed to hard-coded reading,
        /// while pure reflection is 4-5x as slow.
        /// </remarks>
        public static Func<IDataRecord, T> GetMapper<T>() where T : new()
        {
            Delegate mapperDelegate;
            if (!ExpressionCache.TryGetValue(typeof(T), out mapperDelegate))
            {
                var indexerProperty = typeof(IDataRecord).GetProperty("Item", new[] { typeof(string) });
                var statements = new List<Expression>();
                var instanceParam = Expression.Variable(typeof(T));
                var readerParam = Expression.Parameter(typeof(IDataRecord));
                var createInstance = Expression.Assign(instanceParam, Expression.New(typeof(T)));
                statements.Add(createInstance);
                foreach (var property in typeof(T).GetProperties())
                {
                    var getProperty = Expression.Property(instanceParam, property);
                    var readValue = Expression.MakeIndex(readerParam, indexerProperty,
                        new[] { Expression.Constant(property.Name) });
                    // This try catch is handy for when invalid casts occur. Doesn't affect performance too much.
                    var assignProperty = Expression.TryCatch(
                        Expression.Block(typeof(void),
                            Expression.IfThenElse(
                                Expression.Equal(readValue, Expression.Constant(DBNull.Value)),
                                Expression.Assign(getProperty, Expression.Convert(Expression.Constant(null), property.PropertyType)),
                                Expression.Assign(getProperty, Expression.Convert(readValue, property.PropertyType))
                                )
                        ),
                        Expression.Catch(typeof(InvalidCastException),
                                Expression.Throw(Expression.Constant(new InvalidCastException(property.Name))))
                    );
                    statements.Add(assignProperty);
                }
                var returnStatement = instanceParam;
                statements.Add(returnStatement);
                var body = Expression.Block(instanceParam.Type, new[] { instanceParam }, statements.ToArray());
                var lambda = Expression.Lambda<Func<IDataRecord, T>>(body, readerParam);
                mapperDelegate = lambda.Compile();
                ExpressionCache[typeof(T)] = mapperDelegate;
            }
            return (Func<IDataRecord, T>)mapperDelegate;
        }
    }
}
