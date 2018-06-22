using Blazor.FlexGrid.Components;
using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blazor.FlexGrid
{
    public static class FlexGridServiceCollectionExtensions
    {
        public static IServiceCollection AddFlexGrid(this IServiceCollection services, Action<IModelConfiguration> configureGridComponents = null)
        {
            var modelBuilder = new ModelBuilder();
            configureGridComponents?.Invoke(modelBuilder);

            services.AddSingleton(typeof(ILazyDataSetLoader<>), typeof(HttpLazyDataSetLoader<>));
            services.AddSingleton(typeof(LazyLoadedTableDataAdapter<>));
            services.AddSingleton(typeof(IGridComponentsContext), new GridComponentsContext(modelBuilder.Model));
            services.AddSingleton<IGridRenderer, GridHeaderRenderer>();
            services.AddSingleton<IGridRenderer, GridBodyRenderer>();

            return services;
        }
    }
}
