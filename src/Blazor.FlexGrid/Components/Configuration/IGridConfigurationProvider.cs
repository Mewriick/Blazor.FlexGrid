using Blazor.FlexGrid.Components.Configuration.MetaData;
using System;

namespace Blazor.FlexGrid.Components.Configuration
{
    public interface IGridConfigurationProvider
    {
        IEntityType FindGridConfigurationByType(Type clrType);
    }
}
