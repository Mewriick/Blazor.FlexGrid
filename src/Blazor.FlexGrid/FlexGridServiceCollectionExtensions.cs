using Blazor.Extensions.Logging;
using Blazor.FlexGrid.Components;
using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.Builders;
using Blazor.FlexGrid.Components.Configuration.MetaData.Conventions;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
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
        public static IServiceCollection AddFlexGridServerSide(this IServiceCollection services, Action<IModelConfiguration> configureGridComponents = null)
            => services.AddFlexGrid(configureGridComponents, true);

        public static IServiceCollection AddFlexGrid(this IServiceCollection services, Action<IModelConfiguration> configureGridComponents = null, bool isServerSideBlazorApp = false)
        {
            var modelBuilder = new ModelBuilder();
            configureGridComponents?.Invoke(modelBuilder);

            if (isServerSideBlazorApp)
            {
                services.AddLogging(builder => builder.AddConsole());
            }
            else
            {
                services.AddLogging(builder => builder
                    .AddBrowserConsole()
                    .SetMinimumLevel(LogLevel.Debug));
            }

            services.AddSingleton(typeof(ILazyDataSetLoader<>), typeof(HttpLazyDataSetLoader<>));
            services.AddSingleton(typeof(MasterTableDataAdapterBuilder<>));
            services.AddSingleton(typeof(LazyLoadedTableDataAdapter<>));
            services.AddSingleton(typeof(IGridConfigurationProvider), new GridConfigurationProvider(modelBuilder.Model));
            services.AddSingleton<GridRendererContextFactory>();
            services.AddSingleton<IMasterDetailTableDataSetFactory, MasterDetailTableDataSetFactory>();
            services.AddSingleton<ConventionsSet>();
            services.AddSingleton<IPropertyValueAccessorCache, PropertyValueAccessorCache>();
            services.AddSingleton<IDetailDataAdapterVisitors, DetailDataAdapterVisitors>();
            services.AddSingleton<ITableDataAdapterProvider, RunTimeTableDataAdapterProvider>();

            RegisterGridRendererTree(services);

            return services;
        }

        private static void RegisterGridRendererTree(IServiceCollection services)
        {
            services.AddSingleton(typeof(IGridRenderer), provider =>
            {
                var gridRowRenderer = new GridRowRenderer();
                gridRowRenderer.AddRenderer(new GridCellMasterActionRenderer());
                gridRowRenderer.AddRenderer(new GridCellRenderer());
                gridRowRenderer.AddRenderer(new GridTabControlRenderer(provider.GetRequiredService<ITableDataAdapterProvider>()), RendererType.AfterTag);

                var gridBodyRenderer = new GridBodyRenderer(provider.GetRequiredService<ILogger<GridBodyRenderer>>());
                gridBodyRenderer.AddRenderer(gridRowRenderer);

                var gridRenderer = new GridRenderer(provider.GetRequiredService<ILogger<GridRenderer>>());
                gridRenderer.AddRenderer(new GridMesurablePartRenderer(
                        new GridHeaderRenderer(provider.GetRequiredService<ILogger<GridHeaderRenderer>>()),
                        provider.GetRequiredService<ILogger<GridMesurablePartRenderer>>())
                    );

                gridRenderer.AddRenderer(new GridLoadingRenderer(), RendererType.BeforeTag);
                gridRenderer.AddRenderer(new GridMesurablePartRenderer(gridBodyRenderer, provider.GetRequiredService<ILogger<GridMesurablePartRenderer>>()));
                gridRenderer.AddRenderer(new GridPaginationRenderer(), RendererType.AfterTag);

                return gridRenderer;
            });
        }
    }
}
