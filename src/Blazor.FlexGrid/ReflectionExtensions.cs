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

        public static IOrderedQueryable<T> ApplyOrder<T>(this IQueryable<T> source, string property, string methodName)
        {
            var props = property.Split('.');
            var type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                var pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }
}
