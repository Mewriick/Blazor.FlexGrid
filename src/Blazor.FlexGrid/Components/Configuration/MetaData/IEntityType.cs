using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public interface IEntityType : IAnnotatable
    {
        IEnumerable<IProperty> GetProperties();

        IProperty FindProperty(string name);

        IProperty AddProperty(string name, Type type);
    }
}
