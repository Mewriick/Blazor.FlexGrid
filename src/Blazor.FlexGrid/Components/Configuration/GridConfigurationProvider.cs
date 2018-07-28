using Blazor.FlexGrid.Components.Configuration.MetaData;
using System;

namespace Blazor.FlexGrid.Components.Configuration
{
    public class GridConfigurationProvider : IGridConfigurationProvider
    {
        private readonly IModel model;

        public GridConfigurationProvider(IModel model)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public IEntityType FindGridEntityConfigurationByType(Type clrType)
            => model.FindEntityType(clrType) ?? NullEntityType.Instance;

        public IGridViewAnotations GetGridConfigurationByType(Type clrType)
            => new GridAnotations(FindGridEntityConfigurationByType(clrType));
    }
}
