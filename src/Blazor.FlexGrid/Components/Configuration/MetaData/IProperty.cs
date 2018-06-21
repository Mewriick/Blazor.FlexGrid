using System;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public interface IProperty : IAnnotatable
    {
        string Name { get; }

        Type ClrType { get; }

        PropertyInfo PropertyInfo { get; }
    }
}
