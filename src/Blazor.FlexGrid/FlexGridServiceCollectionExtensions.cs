using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.FlexGrid
{
    public static class FlexGridServiceCollectionExtensions
    {
        public static IServiceCollection AddFlexGrid(this IServiceCollection services)
        {
            services.AddSingleton(typeof(ILazyDataSetLoader<>), typeof(HttpLazyDataSetLoader<>));
            services.AddSingleton(typeof(LazyLoadedTableDataAdapter<>));

            return services;
        }
    }
}
