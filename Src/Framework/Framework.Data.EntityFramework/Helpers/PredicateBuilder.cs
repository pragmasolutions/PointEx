using System;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Data.EntityFramework.Helpers
{
    public static class PredicateBuilder
    {
        /// <summary>
        /// Create expression for .Where(entity => entity.Id == 'id')
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="id">Entity ID</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetByIdPredicate<T>(object id)
        {
            // Find primary key property based on primary key attribute.
            var keyProperty = typeof(T).GetProperties().
                First(
                    one =>
                    one.GetCustomAttributes(typeof(EdmScalarPropertyAttribute), true)
                    .Any(two => ((EdmScalarPropertyAttribute)two).EntityKeyProperty));

            // Create entity => portion of lambda expression
            ParameterExpression parameter = Expression.Parameter(typeof(T), "entity");

            // create entity.Id portion of lambda expression
            MemberExpression property = Expression.Property(parameter, keyProperty.Name);

            // create 'id' portion of lambda expression
            var equalsTo = Expression.Constant(id);

            // create entity.Id == 'id' portion of lambda expression
            var equality = Expression.Equal(property, equalsTo);

            // finally create entire expression - entity => entity.Id == 'id'
            Expression<Func<T, bool>> retVal =
                Expression.Lambda<Func<T, bool>>(equality, new[] { parameter });

            return retVal;
        }
    }
}