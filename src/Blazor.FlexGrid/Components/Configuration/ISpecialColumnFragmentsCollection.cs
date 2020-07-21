using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Renderers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
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

    public interface ISpecialColumnFragmentsCollection
    {
        void AddColumnValueRenderFunction(
            Type itemType,
            string columnName,
            IRenderFragmentAdapter renderFragment);

        void AddColumnEditValueRenderer(
            Type itemType,
            string columnName,
            Func<EditColumnContext, IRenderFragmentAdapter> renderFragmentBuilder);

        IReadOnlyDictionary<string, IRenderFragmentAdapter> Merge(
            IEntityType entityType,
            IReadOnlyDictionary<string, IRenderFragmentAdapter> imutableColumnValueFragments);

        IReadOnlyDictionary<string, Func<EditColumnContext, IRenderFragmentAdapter>> Merge(
            IEntityType entityType,
            IReadOnlyDictionary<string, Func<EditColumnContext, IRenderFragmentAdapter>> imutableColumnEditFragments);
    }
}
