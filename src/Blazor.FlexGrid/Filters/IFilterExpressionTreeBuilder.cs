using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Filters
{
    public interface IFilterExpressionTreeBuilder<TItem> where TItem : class
    {
        Expression<Func<TItem, bool>> BuildExpressionTree(IReadOnlyCollection<IFilterDefinition> filters);
    }
}
