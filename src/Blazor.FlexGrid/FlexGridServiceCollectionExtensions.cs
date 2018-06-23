using Blazor.Extensions.Logging;
using Blazor.FlexGrid.Components;
using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.Builders;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Blazor.FlexGrid
{
    public static class FlexGridServiceCollectionExtensions
    {
        public static IServiceCollection AddFlexGrid(this IServiceCollection services, Action<IModelConfiguration> configureGridComponents = null)
        {
            var modelBuilder = new ModelBuilder();
            configureGridComponents?.Invoke(modelBuilder);

            services.AddLogging(builder => builder
                .AddBrowserConsole()
                .SetMinimumLevel(LogLevel.Debug));

            services.AddSingleton(typeof(ILazyDataSetLoader<>), typeof(HttpLazyDataSetLoader<>));
            services.AddSingleton(typeof(LazyLoadedTableDataAdapter<>));
            services.AddSingleton(typeof(IGridConfigurationProvider), new GridComponentsContext(modelBuilder.Model));
            services.AddSingleton<GridRendererContextFactory>();

            services.AddSingleton(typeof(IGridRenderer), provider =>
            {
                var gridRenderer = new GridRenderer(provider.GetRequiredService<ILogger<GridRenderer>>());
                gridRenderer.AddRenderer(new GridMesurablePartRenderer(
                        new GridHeaderRenderer(provider.GetRequiredService<ILogger<GridHeaderRenderer>>()),
                        provider.GetRequiredService<ILogger<GridMesurablePartRenderer>>())
                    );

                gridRenderer.AddRenderer(new GridMesurablePartRenderer(
                       new GridBodyRenderer(provider.GetRequiredService<ILogger<GridBodyRenderer>>()),
                       provider.GetRequiredService<ILogger<GridMesurablePartRenderer>>())
                    );

                gridRenderer.AddRenderer(new GridLoadingRenderer());
                gridRenderer.AddRenderer(new GridPaginationRenderer());

                return gridRenderer;
            });

            return services;
        }
    }
}
