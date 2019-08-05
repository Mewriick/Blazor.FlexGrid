using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Configuration
{
    public interface ISpecialColumnFragmentsCollection<TItem>
    {
        ISpecialColumnFragmentsCollection<TItem> AddColumnValueRenderFunction<TColumn>(
            Expression<Func<TItem, TColumn>> columnExpression,
            RenderFragment<TItem> renderFragment);

        ISpecialColumnFragmentsCollection<TItem> AddColumnEditValueRenderer<TColumn>(
            Expression<Func<TItem, TColumn>> columnExpression,
            Func<EditColumnContext, RenderFragment<TItem>> renderFragmentBuilder);
    }
}
