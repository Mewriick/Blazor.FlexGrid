using Blazor.FlexGrid.Components;
using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.Builders;
using Blazor.FlexGrid.Components.Configuration.MetaData.Conventions;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.Components.Renderers.CreateItemForm;
using Blazor.FlexGrid.Components.Renderers.EditInputs;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataAdapters.Visitors;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Http;
using Blazor.FlexGrid.Permission;
using Blazor.FlexGrid.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
                services.AddScoped(typeof(ILazyDataSetLoader<>), typeof(HttpLazyDataSetLoader<>));
                services.AddScoped(typeof(ILazyDataSetItemManipulator<>), typeof(HttpLazyDataSetItemManipulator<>));
            }
            else
            {
                /*services.AddLogging(builder => builder
                    .AddBrowserConsole()
                    .SetMinimumLevel(LogLevel.Debug));*/

                services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
                services.AddSingleton(typeof(ILazyDataSetLoader<>), typeof(HttpLazyDataSetLoader<>));
                services.AddSingleton(typeof(ILazyDataSetItemManipulator<>), typeof(HttpLazyDataSetItemManipulator<>));
            }

            if (flexGridOptions.UseAuthorizationForHttpRequests)
            {
                services.AddSingleton<IHttpClientFactory, AuthorizationHttpClientFactory>();
            }
            else
            {
                services.AddSingleton<IHttpClientFactory, DefaultHttpClientFactory>();
            }

            services.TryAddSingleton<IAuthorizationService, NullAuthorizationService>();
            services.TryAddSingleton<ICurrentUserPermission>(new NullCurrentUserPermission());
            services.AddSingleton(typeof(MasterTableDataAdapterBuilder<>));
            services.AddSingleton(typeof(LazyLoadedTableDataAdapter<>));
            services.AddSingleton(typeof(IGridConfigurationProvider), new GridConfigurationProvider(modelBuilder.Model));
            services.AddSingleton<IMasterDetailTableDataSetFactory, MasterDetailTableDataSetFactory>();
            services.AddSingleton<ConventionsSet>();
            services.AddSingleton<ITypePropertyAccessorCache, PropertyValueAccessorCache>();
            services.AddSingleton<IDetailDataAdapterVisitors, DetailDataAdapterVisitors>();
            services.AddSingleton<ITableDataAdapterProvider, RunTimeTableDataAdapterProvider>();
            services.AddSingleton<IModelValidator, ValidatableObjectAdapter>();

            RegisterGridRendererTree(services);

            return services;
        }

        private static void RegisterGridRendererTree(IServiceCollection services)
        {
            services.AddSingleton(typeof(CreateItemFormRenderer<>));
            services.AddSingleton(typeof(BlazorComponentColumnCollection<>));
            services.AddSingleton<GridContextsFactory>();
            services.AddSingleton<EditInputRendererTree>();

            services.AddSingleton(typeof(IGridRendererTreeBuilder), provider =>
            {
                var measurableLogger = provider.GetRequiredService<ILogger<GridMesurablePartRenderer>>();

                var gridRowRenderer = new GridRowRenderer()
                    .AddRenderer(new GridCellMasterActionRenderer())
                    .AddRenderer(new GridCellRenderer(provider.GetRequiredService<EditInputRendererTree>()))
                    .AddRenderer(new GridTabControlRenderer(provider.GetRequiredService<ITableDataAdapterProvider>()), RendererType.AfterTag)
                    .AddRenderer(new GridActionButtonsRenderer());

                var gridBodyRenderer = new GridBodyRenderer(provider.GetRequiredService<ILogger<GridBodyRenderer>>())
                    .AddRenderer(gridRowRenderer);

                var gridRenderer = new GridMesurablePartRenderer(
                        new GridRenderer(provider.GetRequiredService<ILogger<GridRenderer>>()), measurableLogger)
                    .AddRenderer(new GridLoadingRenderer(), RendererType.BeforeTag)
                    .AddRenderer(new GridMesurablePartRenderer(new GridHeaderRenderer(), measurableLogger))
                    .AddRenderer(new GridMesurablePartRenderer(gridBodyRenderer, measurableLogger))
                    .AddRenderer(new GridFooterRenderer(), RendererType.AfterTag)
                    .AddRenderer(new CreateItemModalRenderer(), RendererType.AfterTag);

                return gridRenderer;
            });
        }
    }
}
