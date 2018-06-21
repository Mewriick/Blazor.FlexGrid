using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public interface IModel : IAnnotatable
    {
        IEntityType AddEntityType(Type clrType);

        IEnumerable<IEntityType> GetEntityTypes();

        IEntityType FindEntityType(Type clrType);
    }
}
