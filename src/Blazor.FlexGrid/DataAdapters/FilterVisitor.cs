using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.DataAdapters
{
    public class FilterVisitor<TItem> : IDataTableAdapterVisitor where TItem : class
    {
        private readonly Expression<Func<TItem, bool>> filter;

        public FilterVisitor(BinaryExpression binaryExpression, ParameterExpression parameterExpression)
        {
            this.filter = Expression.Lambda<Func<TItem, bool>>(binaryExpression, parameterExpression);
        }

        public void Visit(ITableDataAdapter tableDataAdapter)
        {
            if (tableDataAdapter is CollectionTableDataAdapter<TItem> collectionTableDataAdapter)
            {
                collectionTableDataAdapter.Filter = filter;
            }
        }
    }
}
