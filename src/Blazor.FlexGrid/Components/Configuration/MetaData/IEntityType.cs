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

        IReadOnlyCollection<PropertyInfo> ClrTypeCollectionProperties { get; }

        IEnumerable<IProperty> GetProperties();

        IProperty FindProperty(string name);

        IMasterDetailRelationship FindDetailRelationship(Type detailType);

        IProperty AddProperty(MemberInfo memberInfo);
    }
}
