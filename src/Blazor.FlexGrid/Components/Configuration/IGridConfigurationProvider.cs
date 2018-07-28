using Blazor.FlexGrid.Components.Configuration.MetaData;
using System;

namespace Blazor.FlexGrid.Components.Configuration
{
    public interface IGridConfigurationProvider
    {
        IEntityType FindGridEntityConfigurationByType(Type clrType);

        IGridViewAnotations GetGridConfigurationByType(Type clrType);
    }
}
