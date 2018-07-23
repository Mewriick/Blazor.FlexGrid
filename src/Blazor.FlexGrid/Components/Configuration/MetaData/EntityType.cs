using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class EntityType : Annotatable, IEntityType
    {
        private readonly SortedDictionary<string, Property> properties;
        private readonly SortedDictionary<string, MasterDetailRelationship> detailRelationships;

        public Model Model { get; }

        public Type ClrType { get; }

        public string Name { get; }


        public EntityType(Type clrType, Model model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            ClrType = clrType ?? throw new ArgumentNullException(nameof(clrType));
            Name = clrType.FullName;
            this.properties = new SortedDictionary<string, Property>();
            this.detailRelationships = new SortedDictionary<string, MasterDetailRelationship>();
        }

        public IProperty FindProperty(string name)
            => properties.TryGetValue(name, out var property)
                ? property
                : null;

        public IMasterDetailRelationship FindDetailRelationship(Type detailType)
            => detailRelationships.TryGetValue(detailType.FullName, out var masterDetailRelationship)
                ? masterDetailRelationship
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

        public MasterDetailRelationship AddDetailRelationship(Type detailType, string masterPropertyName, string foreignPropertyName)
        {
            if (detailType is null)
            {
                throw new ArgumentNullException(nameof(detailType));
            }

            if (string.IsNullOrWhiteSpace(foreignPropertyName))
            {
                throw new ArgumentNullException(nameof(foreignPropertyName));
            }

            if (string.IsNullOrWhiteSpace(masterPropertyName))
            {
                throw new ArgumentNullException(nameof(masterPropertyName));
            }

            ValidationDetailRelationshipCanBeAdded(detailType);

            var masterDetailRelationship = new MasterDetailRelationship(masterPropertyName, foreignPropertyName);
            detailRelationships.Add(detailType.FullName, masterDetailRelationship);

            return masterDetailRelationship;
        }

        private void ValidationPropertyCanBeAdded(string propertyName)
        {
            if (properties.TryGetValue(propertyName, out var property))
            {
                throw new InvalidOperationException($"The property {propertyName} cannot be configured on type {property.DeclaringType} " +
                    $"because property with same name is already configured");
            }
        }

        private void ValidationDetailRelationshipCanBeAdded(Type detailType)
        {
            if (properties.TryGetValue(detailType.FullName, out var detailRelationship))
            {
                throw new InvalidOperationException($"The detail relationship for {detailType.FullName} cannot be configured on type {ClrType} " +
                    $"because detail relationship for the same type is already configured");
            }
        }

        IModel IEntityType.Model => Model;

        IProperty IEntityType.AddProperty(MemberInfo memberInfo) => AddProperty(memberInfo);
    }
}
