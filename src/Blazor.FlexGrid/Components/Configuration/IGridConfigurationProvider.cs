using Blazor.FlexGrid.Components.Configuration.MetaData;
using System;

namespace Blazor.FlexGrid.Components.Configuration
{
    public interface IGridConfigurationProvider
    {
        IModel ConfigurationModel { get; }

        IEntityType FindGridEntityConfigurationByType(Type clrType);

        IGridViewAnotations GetGridConfigurationByType(Type clrType);
    }
}
