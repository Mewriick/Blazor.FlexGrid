using Blazor.FlexGrid.Components.Configuration.Builders;
using System;

namespace Blazor.FlexGrid.Components.Configuration.MetaData.Conventions
{
    public abstract class BaseConvention : IConvention
    {
        protected readonly InternalModelBuilder internalModelBuilder;
        private readonly IGridConfigurationProvider gridConfigurationProvider;

        public BaseConvention(IGridConfigurationProvider gridConfigurationProvider)
        {
            this.gridConfigurationProvider = gridConfigurationProvider ?? throw new ArgumentNullException(nameof(gridConfigurationProvider));
            this.internalModelBuilder = new InternalModelBuilder(gridConfigurationProvider.ConfigurationModel as Model);
        }

        public void Apply(Type type)
        {
            var entityType = gridConfigurationProvider.FindGridEntityConfigurationByType(type);
            if (entityType is NullEntityType)
            {
                var entityTypeBuilder = internalModelBuilder
                    .Entity(type);

                Apply(entityTypeBuilder);
            }
            else
            {
                Apply(new InternalEntityTypeBuilder(entityType as EntityType, internalModelBuilder));
            }
        }

        public abstract void Apply(InternalEntityTypeBuilder internalEntityTypeBuilder);
    }
}
