using Blazor.FlexGrid.Components.Configuration.MetaData;
using System;

namespace Blazor.FlexGrid.Components.Configuration
{
    public interface IGridComponentsContext
    {
        IEntityType FindGridConfigurationByType(Type clrType);
    }
}
