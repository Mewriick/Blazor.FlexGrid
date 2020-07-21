using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Renderers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Configuration
{
    public class BlazorComponentColumnCollection<TItem> : ISpecialColumnFragmentsCollection<TItem>
    {
        private readonly ISpecialColumnFragmentsCollection specialColumnFragmentsCollection;

        public BlazorComponentColumnCollection(ISpecialColumnFragmentsCollection specialColumnFragmentsCollection)
        {
            this.specialColumnFragmentsCollection = specialColumnFragmentsCollection ?? throw new ArgumentNullException(nameof(specialColumnFragmentsCollection));
        }

        public ISpecialColumnFragmentsCollection<TItem> AddColumnValueRenderFunction<TColumn>(
            Expression<Func<TItem, TColumn>> columnExpression,
            RenderFragment<TItem> renderFragment)
        {
            var columnName = GetColumnName(columnExpression);
            specialColumnFragmentsCollection.AddColumnValueRenderFunction(
                typeof(TItem),
                columnName,
                new RenderFragmentAdapter<TItem>(renderFragment));

            return this;
        }

        public ISpecialColumnFragmentsCollection<TItem> AddColumnEditValueRenderer<TColumn>(
            Expression<Func<TItem, TColumn>> columnExpression,
            Func<EditColumnContext, RenderFragment<TItem>> renderFragmentBuilder)
        {
            var columnName = GetColumnName(columnExpression);

            Func<EditColumnContext, IRenderFragmentAdapter> builder
                = context =>
                {
                    return new RenderFragmentAdapter<TItem>(renderFragmentBuilder.Invoke(context));
                };

            specialColumnFragmentsCollection.AddColumnEditValueRenderer(
                typeof(TItem),
                columnName,
                builder);

            return this;
        }

        private string GetColumnName<TColumn>(Expression<Func<TItem, TColumn>> columnExpression)
            => columnExpression.GetPropertyAccess().Name;
    }

    public class BlazorComponentColumnCollection : ISpecialColumnFragmentsCollection
    {
        private Dictionary<string, IRenderFragmentAdapter> columnValueFragments;
        private Dictionary<string, Func<EditColumnContext, IRenderFragmentAdapter>> columnEditFragments;

        public BlazorComponentColumnCollection()
        {
            this.columnValueFragments = new Dictionary<string, IRenderFragmentAdapter>();
            this.columnEditFragments = new Dictionary<string, Func<EditColumnContext, IRenderFragmentAdapter>>();
        }

        public void AddColumnEditValueRenderer(
            Type itemType,
            string columnName,
            Func<EditColumnContext, IRenderFragmentAdapter> renderFragmentBuilder)
        {
            var key = BuildKey(itemType, columnName);
            columnEditFragments[key] = renderFragmentBuilder;
        }

        public void AddColumnValueRenderFunction(
            Type itemType,
            string columnName,
            IRenderFragmentAdapter renderFragment)
        {
            var key = BuildKey(itemType, columnName);
            columnValueFragments[key] = renderFragment;
        }

        public IReadOnlyDictionary<string, IRenderFragmentAdapter> Merge(
            IEntityType entityType,
            IReadOnlyDictionary<string, IRenderFragmentAdapter> imutableColumnValueFragments)
        {
            if (entityType is null ||
                string.IsNullOrWhiteSpace(entityType?.ClrType?.FullName))
            {
                return new Dictionary<string, IRenderFragmentAdapter>();
            }

            foreach (var fragmentPair in imutableColumnValueFragments)
            {
                var key = BuildKey(entityType.ClrType, fragmentPair.Key);
                if (!columnValueFragments.ContainsKey(key))
                {
                    columnValueFragments[key] = fragmentPair.Value;
                }
            }

            return columnValueFragments.Where(cf => cf.Key.StartsWith(entityType.ClrType.FullName))
                .ToDictionary(pair => GetColumnName(pair.Key), pair => pair.Value);
        }

        public IReadOnlyDictionary<string, Func<EditColumnContext, IRenderFragmentAdapter>> Merge(
            IEntityType entityType,
            IReadOnlyDictionary<string, Func<EditColumnContext, IRenderFragmentAdapter>> imutableColumnEditFragments)
        {
            if (entityType is null ||
                string.IsNullOrWhiteSpace(entityType?.ClrType?.FullName))
            {
                return new Dictionary<string, Func<EditColumnContext, IRenderFragmentAdapter>>();
            }

            foreach (var fragmentPair in imutableColumnEditFragments)
            {
                var key = BuildKey(entityType.ClrType, fragmentPair.Key);
                if (!columnEditFragments.ContainsKey(key))
                {
                    columnEditFragments[key] = fragmentPair.Value;
                }
            }

            return columnEditFragments.Where(cf => cf.Key.StartsWith(entityType.ClrType.FullName))
                .ToDictionary(pair => GetColumnName(pair.Key), pair => pair.Value);
        }

        private static string BuildKey(Type type, string columnName)
            => $"{type.FullName}_{columnName}";

        private static string GetColumnName(string key)
            => key.Split('_')[1];
    }
}
