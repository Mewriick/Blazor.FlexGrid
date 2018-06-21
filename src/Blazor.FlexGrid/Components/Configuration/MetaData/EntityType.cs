using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class EntityType : Annotatable, IEntityType
    {
        private readonly SortedDictionary<string, Property> properties;

        public Model Model { get; }

        public Type ClrType { get; }

        public string Name { get; }


        public EntityType(Type clrType, Model model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Name = clrType.FullName;
            this.properties = new SortedDictionary<string, Property>();
        }

        public IProperty FindProperty(string name)
            => properties.TryGetValue(name, out var property)
                ? property
                : null;

        public IEnumerable<IProperty> GetProperties() => properties.Values;


        public Property AddProperty(MemberInfo memberInfo)
        {
            if (memberInfo is null)
            {
                throw new ArgumentNullException(nameof(memberInfo));
            }

            ValidationPropertyCanBeAdded(memberInfo.Name);

            var property = new Property(memberInfo.Name, memberInfo.GetMemberType(), memberInfo as PropertyInfo, this);
            properties.Add(memberInfo.Name, property);

            return property;
        }


        private void ValidationPropertyCanBeAdded(string propertyName)
        {
            if (properties.TryGetValue(propertyName, out var property))
            {
                throw new InvalidOperationException($"The property {propertyName} cannot be configured on type {property.DeclaringType} " +
                    $"because property with same name is already configured");
            }
        }


        IModel IEntityType.Model => Model;

        IProperty IEntityType.AddProperty(MemberInfo memberInfo) => AddProperty(memberInfo);
    }
}
