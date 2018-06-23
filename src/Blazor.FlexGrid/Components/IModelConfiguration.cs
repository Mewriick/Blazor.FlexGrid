using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.Builders;

namespace Blazor.FlexGrid.Components
{
    public interface IModelConfiguration
    {
        ModelBuilder ApplyConfiguration<TEntity>(IEntityTypeConfiguration<TEntity> entityTypeConfiguration) where TEntity : class;
    }
}
