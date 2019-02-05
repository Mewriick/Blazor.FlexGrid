using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Blazor.FlexGrid
{
    public static class ReflectionExtensions
    {
        public static Type GetMemberType(this MemberInfo memberInfo)
            => (memberInfo as PropertyInfo)?.PropertyType ?? (memberInfo as FieldInfo)?.FieldType;


        public static PropertyInfo GetPropertyAccess(this LambdaExpression propertyAccessExpression)
        {
            var parameterExpression = propertyAccessExpression.Parameters.Single();
            var parameterType = parameterExpression.Type;
            var body = propertyAccessExpression.Body;

            if (!(body is MemberExpression member) &&
                !(body is UnaryExpression unary && (member = unary.Operand as MemberExpression) != null))
                throw new ArgumentException($"Expression '{propertyAccessExpression}' does not refer to a property.");

            if (!(member.Member is PropertyInfo propInfo))
                throw new ArgumentException($"Expression '{propertyAccessExpression}' refers to a field, not a property.");

            if (!propInfo.DeclaringType.GetTypeInfo().IsAssignableFrom(parameterType.GetTypeInfo()))
                throw new ArgumentException($"Expresion '{propertyAccessExpression}' refers to a property that is not from type '{parameterType}'.");

            return propInfo;
        }
    }

    internal static class QueryableExtensions
    {
        private static LambdaExpression GetPropertyExpression(Type type, string property)
        {
            var arg = Expression.Parameter(type);
            var expr = property.Split('.')
                .Aggregate((Expression)arg, Expression.Property);
            return Expression.Lambda(expr, arg);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string keyProperty)
            => OrderBy(nameof(_OrderBy), source, GetPropertyExpression(typeof(T), keyProperty));

        private static IOrderedQueryable<T> _OrderBy<T, TKey>(IQueryable<T> source, LambdaExpression keySelector)
            => source.OrderBy((Expression<Func<T, TKey>>)keySelector);

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string keyProperty)
            => OrderBy(nameof(_OrderByDescending), source, GetPropertyExpression(typeof(T), keyProperty));
        
        private static IOrderedQueryable<T> _OrderByDescending<T, TKey>(IQueryable<T> source, LambdaExpression keySelector)
            => source.OrderByDescending((Expression<Func<T, TKey>>)keySelector);

        private static IOrderedQueryable<T> OrderBy<T>(string funcName, IQueryable<T> source, LambdaExpression keySelector)
            => typeof(QueryableExtensions).GetMethod(funcName, BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(typeof(T), keySelector.Body.Type)
                .CreateDelegate<Func<IQueryable<T>, LambdaExpression, IOrderedQueryable<T>>>()
                .Invoke(source, keySelector);
    }

    internal static class DelegateExtensions
    {
        public static T CreateDelegate<T>(this MethodInfo method) where T : Delegate
            => (T)method.CreateDelegate(typeof(T));
    }
}
