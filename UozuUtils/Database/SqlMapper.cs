using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Uozu.Utils.Database
{
    public class SqlMapper
    {
        private static ConcurrentDictionary<Type, Delegate> ExpressionCache =
                       new ConcurrentDictionary<Type, Delegate>();

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

        /// <summary>
        /// Returns a mapper that reads record into the given data type.
        /// </summary>
        /// <remarks>
        /// Largely inspired by 
        /// http://www.codeproject.com/Articles/503527/Reflection-optimization-techniques
        /// Quick testing shows that this performs at similar speed to hard-coded reading,
        /// while pure reflection is 4-5x as slow.
        /// </remarks>
        private static Func<IDataRecord, T> GetMapper<T>() where T : new()
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
