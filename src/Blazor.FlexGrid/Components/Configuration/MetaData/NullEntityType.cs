using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class NullEntityType : IEntityType
    {
        public static NullEntityType Instance = new NullEntityType();

        public object this[string name] => throw new NotImplementedException();

        public Type ClrType => throw new NotImplementedException();

        public IModel Model => throw new NotImplementedException();

        public string Name => string.Empty;

        public IProperty AddProperty(MemberInfo memberInfo)
        {
            throw new NotImplementedException();
        }

        public IAnnotation FindAnnotation(string name)
        {
            return null;
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
