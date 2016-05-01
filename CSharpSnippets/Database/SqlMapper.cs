using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace CSharpSnippets.Database
{
    public class SqlMapper
    {
        private static ConcurrentDictionary<Type, Delegate> ExpressionCache =
                       new ConcurrentDictionary<Type, Delegate>();

        public static T MapObjectWithCachedReflection<T>(SqlDataReader reader) where T : new()
        {
            var readRow = GetReader<T>();
            return readRow(reader);
        }

        public static T MapObjectWithReflection<T>(SqlDataReader reader) where T : new()
        {
            var t = new T();
            foreach (var property in typeof(T).GetProperties())
            {
                var val = reader[property.Name];
                var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                var cval = (val == null) ? null : Convert.ChangeType(val, type);
                property.SetValue(t, cval);
            }
            return t;
        }

        // from http://www.codeproject.com/Articles/503527/Reflection-optimization-techniques
        private static Func<SqlDataReader, T> GetReader<T>()
        {
            Delegate resDelegate;
            if (!ExpressionCache.TryGetValue(typeof(T), out resDelegate))
            {
                var indexerProperty = typeof(SqlDataReader).GetProperty("Item", new[] { typeof(string) });
                var statements = new List<Expression>();
                var instanceParam = Expression.Variable(typeof(T));
                var readerParam = Expression.Parameter(typeof(SqlDataReader));
                // var instanceParam = new T();
                var createInstance = Expression.Assign(instanceParam, Expression.New(typeof(T)));
                statements.Add(createInstance);

                foreach (var property in typeof(T).GetProperties())
                {
                    var getProperty = Expression.Property(instanceParam, property);
                    var readValue = Expression.MakeIndex(readerParam, indexerProperty,
                        new[] { Expression.Constant(property.Name) });
                    var underType = Nullable.GetUnderlyingType(property.PropertyType);
                    // TODO: this try catch is handy for when invalid casts occur, does it significantly
                    //       impact performance?
                    var assignProperty = Expression.TryCatch(
                        Expression.Block(typeof(void),
                            Expression.Assign(getProperty, Expression.Convert(readValue, property.PropertyType))
                        ),
                        Expression.Catch(typeof(InvalidCastException),
                                Expression.Throw(Expression.Constant(new InvalidCastException(property.Name))))
                    );
                    statements.Add(assignProperty);
                }
                var returnStatement = instanceParam;
                statements.Add(returnStatement);
                var body = Expression.Block(instanceParam.Type, new[] { instanceParam }, statements.ToArray());
                var lambda = Expression.Lambda<Func<SqlDataReader, T>>(body, readerParam);
                resDelegate = lambda.Compile();
                ExpressionCache[typeof(T)] = resDelegate;
            }
            return (Func<SqlDataReader, T>)resDelegate;
        }
    }
}
