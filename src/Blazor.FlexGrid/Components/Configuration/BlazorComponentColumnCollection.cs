using Blazor.FlexGrid.Components.Configuration.Builders;
using Blazor.FlexGrid.Components.Configuration.MetaData;
using Microsoft.AspNetCore.Blazor;
using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Configuration
{
    public class BlazorComponentColumnCollection<TItem> : ISpecialColumnFragmentsCollection<TItem>
    {
        private readonly InternalModelBuilder internalModelBuilder;
        private readonly IGridConfigurationProvider gridConfigurationProvider;

        public BlazorComponentColumnCollection(IGridConfigurationProvider gridConfigurationProvider)
        {
            this.gridConfigurationProvider = gridConfigurationProvider ?? throw new ArgumentNullException(nameof(gridConfigurationProvider));
            this.internalModelBuilder = new InternalModelBuilder(gridConfigurationProvider.ConfigurationModel as Model);
        }

        public ISpecialColumnFragmentsCollection<TItem> AddColumnValueRenderFunction<TColumn>(Expression<Func<TItem, TColumn>> columnExpression, RenderFragment<TItem> renderFragment)
        {
            var entityType = gridConfigurationProvider.FindGridEntityConfigurationByType(typeof(TItem));
            if (entityType is NullEntityType)
            {
                var entityTypeBuilder = internalModelBuilder
                    .Entity(typeof(TItem))
                    .Property(columnExpression.GetPropertyAccess())
                    .HasBlazorComponentValue(renderFragment);

                return this;
            }

            var columnName = columnExpression.GetPropertyAccess().Name;
            var configurationProperty = entityType.FindProperty(columnName);
            if (configurationProperty is Property property)
            {
                new InternalPropertyBuilder(property, internalModelBuilder)
                    .HasBlazorComponentValue(renderFragment);
            }

            return this;
        }
    }
}
