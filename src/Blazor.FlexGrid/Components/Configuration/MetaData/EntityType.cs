using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class EntityType : Annotatable, IEntityType
    {
        private readonly SortedDictionary<string, Property> properties;

        public Model Model { get; }

        public EntityType(Model model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            this.properties = new SortedDictionary<string, Property>();
        }

        public IProperty FindProperty(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IProperty> GetProperties()
        {
            throw new NotImplementedException();
        }

        public Property AddProperty(string name, Type type)
        {
            return AddProperty(name, type);
        }

        IProperty IEntityType.AddProperty(string name, Type type)
        {
            var property = new Property(name, type, this);
            properties.Add(property.Name, property);

            return property;
        }
    }
}
