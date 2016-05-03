using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace CSharpSnippets.Database
{
    public class SqlMapper
    {
        private static ConcurrentDictionary<Type, Delegate> ExpressionCache =
                       new ConcurrentDictionary<Type, Delegate>();

        public static T MapObjectWithCachedReflection<T>(IDataReader reader) where T : new()
        {
            var readRow = GetReader<T>();
            return readRow(reader);
        }

        public static T MapObjectWithReflection<T>(IDataReader reader) where T : new()
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

        // from http://www.codeproject.com/Articles/503527/Reflection-optimization-techniques
        private static Func<IDataReader, T> GetReader<T>()
        {
            Delegate readerDelegate;
            if (!ExpressionCache.TryGetValue(typeof(T), out readerDelegate))
            {
                var indexerProperty = typeof(IDataRecord).GetProperty("Item", new[] { typeof(string) });
                var statements = new List<Expression>();
                var instanceParam = Expression.Variable(typeof(T));
                var readerParam = Expression.Parameter(typeof(IDataReader));
                var createInstance = Expression.Assign(instanceParam, Expression.New(typeof(T)));
                statements.Add(createInstance);
                foreach (var property in typeof(T).GetProperties())
                {
                    var getProperty = Expression.Property(instanceParam, property);
                    var readValue = Expression.MakeIndex(readerParam, indexerProperty,
                        new[] { Expression.Constant(property.Name) });
                    // TODO: this try catch is handy for when invalid casts occur, does it significantly
                    //       impact performance?
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
                var lambda = Expression.Lambda<Func<IDataReader, T>>(body, readerParam);
                readerDelegate = lambda.Compile();
                ExpressionCache[typeof(T)] = readerDelegate;
            }
            return (Func<IDataReader, T>)readerDelegate;
        }
    }
}
