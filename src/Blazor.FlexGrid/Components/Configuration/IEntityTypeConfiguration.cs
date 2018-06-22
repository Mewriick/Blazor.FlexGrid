namespace Blazor.FlexGrid.Components.Configuration
{
    public interface IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        void Configure(EntityTypeBuilder<TEntity> builder);
    }
}
