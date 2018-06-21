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
}
