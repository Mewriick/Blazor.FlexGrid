using System;

namespace Blazor.FlexGrid.Components.Configuration
{
    public class EntityTypeBuilder<TEntity> where TEntity : class
    {
        private InternalEntityTypeBuilder Builder { get; }

        public EntityTypeBuilder(InternalEntityTypeBuilder internalEntityTypeBuilder)
        {
            Builder = internalEntityTypeBuilder ?? throw new ArgumentNullException(nameof(internalEntityTypeBuilder));
        }

        //public virtual PropertyBuilder<TProperty> Property<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression)
        //{
        //    return new PropertyBuilder<TProperty>(Builder.Property());
        //}

        public virtual PropertyBuilder<TProperty> Property<TProperty>(string name, Type propertyType)
        {
            return new PropertyBuilder<TProperty>(Builder.Property(name, propertyType));
        }
    }
}
