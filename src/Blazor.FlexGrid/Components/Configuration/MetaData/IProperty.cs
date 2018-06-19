using System;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public interface IProperty : IAnnotatable
    {
        string Name { get; }

        Type ClrType { get; }
    }
}
