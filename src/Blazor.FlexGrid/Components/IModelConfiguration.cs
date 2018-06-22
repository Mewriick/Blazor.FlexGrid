using Blazor.FlexGrid.Components.Configuration;

namespace Blazor.FlexGrid.Components
{
    public interface IModelConfiguration
    {
        ModelBuilder ApplyConfiguration<TEntity>(IEntityTypeConfiguration<TEntity> entityTypeConfiguration) where TEntity : class;
    }
}
