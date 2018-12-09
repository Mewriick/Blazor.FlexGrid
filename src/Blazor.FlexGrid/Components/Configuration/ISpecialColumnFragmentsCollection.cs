using Microsoft.AspNetCore.Blazor;
using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Configuration
{
    public interface ISpecialColumnFragmentsCollection<TItem>
    {
        void AddColumnValueRenderFunction<TColumn>(Expression<Func<TItem, TColumn>> columnExpression, RenderFragment<TItem> renderFragment);
    }
}
