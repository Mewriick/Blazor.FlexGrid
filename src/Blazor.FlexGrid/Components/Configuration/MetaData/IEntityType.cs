using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public interface IEntityType : IAnnotatable
    {
        Type ClrType { get; }

        IModel Model { get; }

        string Name { get; }

        IEnumerable<IProperty> GetProperties();

        IProperty FindProperty(string name);

        IProperty AddProperty(MemberInfo memberInfo);
    }
}
