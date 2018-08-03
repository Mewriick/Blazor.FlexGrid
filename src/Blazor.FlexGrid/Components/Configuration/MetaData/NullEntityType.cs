using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class NullEntityType : IEntityType
    {
        public static NullEntityType Instance = new NullEntityType();

        public object this[string name] => FindAnnotation(name).Value;

        public Type ClrType => null;

        public IModel Model => null;

        public string Name => string.Empty;

        public IReadOnlyCollection<PropertyInfo> ClrTypeCollectionProperties => new List<PropertyInfo>();

        public IProperty AddProperty(MemberInfo memberInfo)
        {
            return null;
        }

        public IMasterDetailRelationship FindDetailRelationship(Type detailType)
            => null;

        public IAnnotation FindAnnotation(string name)
        {
            return NullAnnotation.Instance;
        }

        public IProperty FindProperty(string name)
        {
            return null;
        }

        public IEnumerable<IAnnotation> GetAllAnnotations()
        {
            return new List<Annotation>();
        }

        public IEnumerable<IProperty> GetProperties()
        {
            return new List<Property>();
        }
    }
}
