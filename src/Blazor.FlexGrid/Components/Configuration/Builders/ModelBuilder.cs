using Blazor.FlexGrid.Components.Configuration.MetaData;
using System;

namespace Blazor.FlexGrid.Components.Configuration.Builders
{
    public class ModelBuilder : IModelConfiguration
    {
        private readonly InternalModelBuilder builder;

        public IModel Model => builder.Metadata;

        public ModelBuilder()
        {
            builder = new InternalModelBuilder(new Model());
        }

        public EntityTypeBuilder<TEntity> Entity<TEntity>() where TEntity : class
            => new EntityTypeBuilder<TEntity>(builder.Entity(typeof(TEntity)));

        public EntityTypeBuilder<TEntity> Entity<TEntity>(Type type) where TEntity : class
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return new EntityTypeBuilder<TEntity>(builder.Entity(type));
        }

        public ModelBuilder ApplyConfiguration<TEntity>(IEntityTypeConfiguration<TEntity> entityTypeConfiguration) where TEntity : class
        {
            if (entityTypeConfiguration is null)
            {
                throw new ArgumentNullException(nameof(entityTypeConfiguration));
            }

            entityTypeConfiguration.Configure(Entity<TEntity>());

            return this;
        }
    }
}
