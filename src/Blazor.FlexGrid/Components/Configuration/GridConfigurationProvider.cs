using Blazor.FlexGrid.Components.Configuration.MetaData;
using System;

namespace Blazor.FlexGrid.Components.Configuration
{
    public class GridConfigurationProvider : IGridConfigurationProvider
    {
        public IModel ConfigurationModel { get; }


        public GridConfigurationProvider(IModel model)
        {
            ConfigurationModel = model ?? throw new ArgumentNullException(nameof(model));
        }

        public IEntityType FindGridEntityConfigurationByType(Type clrType)
            => ConfigurationModel.FindEntityType(clrType) ?? NullEntityType.Instance;

        public IGridViewAnotations GetGridConfigurationByType(Type clrType)
            => new GridAnotations(FindGridEntityConfigurationByType(clrType));
    }
}
