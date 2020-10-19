﻿using System;
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

            switch (filter.FilterOperation)
            {
                case FilterOperation.Equal:
                    return valueType == typeof(string)
                        ? Expression.Call(memberExpression, EqualsMethod, constantExpression, stringComparisonExpression)
                        : (Expression)Expression.Equal(memberExpression, constantExpression);
                case FilterOperation.GreaterThan:
                    return Expression.GreaterThan(memberExpression, constantExpression);
                case FilterOperation.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(memberExpression, constantExpression);
                case FilterOperation.LessThan:
                    return Expression.LessThan(memberExpression, constantExpression);
                case FilterOperation.LessThanOrEqual:
                    return Expression.LessThanOrEqual(memberExpression, constantExpression);
                case FilterOperation.NotEqual:
                    return valueType == typeof(string)
                        ? Expression.Not(Expression.Call(memberExpression, EqualsMethod, constantExpression, stringComparisonExpression))
                        : (Expression)Expression.NotEqual(memberExpression, constantExpression);
                case FilterOperation.Contains:
                    return Expression.Call(memberExpression, ContainsMethod, constantExpression, stringComparisonExpression);
                case FilterOperation.StartsWith:
                    return Expression.Call(memberExpression, StartsWithMethod, constantExpression, stringComparisonExpression);
                case FilterOperation.EndsWith:
                    return Expression.Call(memberExpression, EndsWithMethod, constantExpression, stringComparisonExpression);
                default:
                    throw new InvalidOperationException($"Conversion is not defined for operation {filter.FilterOperation}");
            }
        }
    }
}
