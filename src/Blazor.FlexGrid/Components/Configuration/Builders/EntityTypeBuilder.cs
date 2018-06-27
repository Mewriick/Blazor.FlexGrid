using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Configuration.Builders
{
    public class EntityTypeBuilder<TEntity> where TEntity : class
    {
        private InternalEntityTypeBuilder Builder { get; }

        public EntityTypeBuilder(InternalEntityTypeBuilder internalEntityTypeBuilder)
        {
            Builder = internalEntityTypeBuilder ?? throw new ArgumentNullException(nameof(internalEntityTypeBuilder));
        }

        public virtual PropertyBuilder<TProperty> Property<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression)
            =>
             new PropertyBuilder<TProperty>(
                Builder.Property(propertyExpression.GetPropertyAccess())
                );

        public virtual EntityTypeBuilder<TEntity> UseCssClasses(Action<GridCssClasses> configureCssClasses)
        {
            Builder.UseCssClasses(configureCssClasses);

            return this;
        }
    }
}
