using Blazor.Extensions.Logging;
using Blazor.FlexGrid.Components;
using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.Builders;
using Blazor.FlexGrid.Components.Configuration.MetaData.Conventions;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataAdapters.Visitors;
using Blazor.FlexGrid.DataSet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Blazor.FlexGrid
{
    public static class FlexGridServiceCollectionExtensions
    {
        public static IServiceCollection AddFlexGridServerSide(this IServiceCollection services, Action<IModelConfiguration> configureGridComponents = null)
            => services.AddFlexGrid(configureGridComponents, options =>
            {
                options.IsServerSideBlazorApp = true;
            });

        public static IServiceCollection AddFlexGrid(
            this IServiceCollection services,
            Action<IModelConfiguration> configureGridComponents = null,
            Action<FlexGridOptions> configureOptions = null)
        {
            var modelBuilder = new ModelBuilder();
            var flexGridOptions = new FlexGridOptions();
            configureGridComponents?.Invoke(modelBuilder);
            configureOptions?.Invoke(flexGridOptions);

            if (flexGridOptions.IsServerSideBlazorApp)
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
            services.AddSingleton(typeof(BlazorComponentColumnCollection<>));
            services.AddSingleton<GridRendererContextFactory>();

            services.AddSingleton(typeof(IGridRenderer), provider =>
            {
                var measurableLogger = provider.GetRequiredService<ILogger<GridMesurablePartRenderer>>();

                var gridRowRenderer = new GridRowRenderer()
                    .AddRenderer(new GridCellMasterActionRenderer())
                    .AddRenderer(new GridCellRenderer())
                    .AddRenderer(new GridTabControlRenderer(provider.GetRequiredService<ITableDataAdapterProvider>()), RendererType.AfterTag);

                var gridBodyRenderer = new GridBodyRenderer(provider.GetRequiredService<ILogger<GridBodyRenderer>>())
                    .AddRenderer(gridRowRenderer);

                var gridRenderer = new GridMesurablePartRenderer(
                        new GridRenderer(provider.GetRequiredService<ILogger<GridRenderer>>()), measurableLogger)
                    .AddRenderer(new GridLoadingRenderer(), RendererType.BeforeTag)
                    .AddRenderer(new GridMesurablePartRenderer(new GridHeaderRenderer(), measurableLogger))
                    .AddRenderer(new GridMesurablePartRenderer(gridBodyRenderer, measurableLogger))
                    .AddRenderer(new GridPaginationRenderer(), RendererType.AfterTag);

                return gridRenderer;
            });
        }
    }
}
