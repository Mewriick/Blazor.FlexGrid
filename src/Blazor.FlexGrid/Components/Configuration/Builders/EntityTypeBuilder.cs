using System;
using System.Collections.Generic;
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
            => new PropertyBuilder<TProperty>(
                    Builder.Property(propertyExpression.GetPropertyAccess())
                );

        public virtual MasterDetailRelationshipBuilder HasDetailRelationship<TDetailEntity>
            (Expression<Func<TEntity, object>> propertyKeyExpression, Expression<Func<TDetailEntity, object>> propertyDetailExpression)
            => new MasterDetailRelationshipBuilder(
                    Builder.HasDetailRelationship(typeof(TDetailEntity), propertyKeyExpression.GetPropertyAccess().Name, propertyDetailExpression.GetPropertyAccess().Name)
                    );

        public virtual MasterDetailRelationshipBuilder HasDetailRelationship<TDetailEntity>(Expression<Func<TEntity, IEnumerable<TDetailEntity>>> collectionProperty)
            => new MasterDetailRelationshipBuilder(
                    Builder.HasDetailRelationship(typeof(TDetailEntity))
                );

        public virtual EntityTypeBuilder<TEntity> UseCssClasses(Action<GridCssClasses> configureCssClasses)
        {
            Builder.UseCssClasses(configureCssClasses);

            return this;
        }

        public virtual EntityTypeBuilder<TEntity> IsMasterTable()
        {
            Builder.IsMasterTable();

            return this;
        }
    }
}
