using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Blazor.FlexGrid.Filters
{
    public class FilterConverter
    {
        private static readonly MethodInfo ContainsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string), typeof(StringComparison) });
        private static readonly MethodInfo StartsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string), typeof(StringComparison) });
        private static readonly MethodInfo EndsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string), typeof(StringComparison) });
        private static readonly MethodInfo EqualsMethod = typeof(string).GetMethod("Equals", new Type[] { typeof(string), typeof(StringComparison) });

        public static Expression ConvertToExpression<TParameter>(IFilterDefinition filter, ParameterExpression parameter)
            where TParameter : class
        {
            var memberExpression = Expression.Property(parameter, filter.ColumnName);
            var constantExpression = Expression.Constant(filter.Value);
            var stringComparisonExpression = Expression.Constant(filter.TextComparasion);
            var valueType = filter.Value.GetType();

            var condition = default(Expression);
            switch (filter.FilterOperation)
            {
                case FilterOperation.Equal:
                    condition = valueType == typeof(string)
                        ? Expression.Call(memberExpression, EqualsMethod, constantExpression, stringComparisonExpression)
                        : (Expression)Expression.Equal(memberExpression, constantExpression);
                    break;
                case FilterOperation.GreaterThan:
                    condition = Expression.GreaterThan(memberExpression, constantExpression);
                    break;
                case FilterOperation.GreaterThanOrEqual:
                    condition = Expression.GreaterThanOrEqual(memberExpression, constantExpression);
                    break;
                case FilterOperation.LessThan:
                    condition = Expression.LessThan(memberExpression, constantExpression);
                    break;
                case FilterOperation.LessThanOrEqual:
                    condition = Expression.LessThanOrEqual(memberExpression, constantExpression);
                    break;
                case FilterOperation.NotEqual:
                    condition = valueType == typeof(string)
                        ? Expression.Not(Expression.Call(memberExpression, EqualsMethod, constantExpression, stringComparisonExpression))
                        : (Expression)Expression.NotEqual(memberExpression, constantExpression);
                    break;
                case FilterOperation.Contains:
                    condition = Expression.Call(memberExpression, ContainsMethod, constantExpression, stringComparisonExpression);
                    break;
                case FilterOperation.StartsWith:
                    condition = Expression.Call(memberExpression, StartsWithMethod, constantExpression, stringComparisonExpression);
                    break;
                case FilterOperation.EndsWith:
                    condition = Expression.Call(memberExpression, EndsWithMethod, constantExpression, stringComparisonExpression);
                    break;
                default:
                    throw new InvalidOperationException($"Conversion is not defined for operation {filter.FilterOperation}");
            }

            if (!valueType.IsValueType)
            {
                var nullCheck = Expression.NotEqual(memberExpression, Expression.Constant(null, typeof(object)));
                condition = Expression.AndAlso(nullCheck, condition);
            }

            return condition;
        }
    }
}
