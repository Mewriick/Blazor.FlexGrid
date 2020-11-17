﻿using Blazor.FlexGrid.Components;
using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.Builders;
using Blazor.FlexGrid.Components.Configuration.MetaData.Conventions;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.Components.Renderers.CreateItemForm;
using Blazor.FlexGrid.Components.Renderers.CreateItemForm.Layouts;
using Blazor.FlexGrid.Components.Renderers.EditInputs;
using Blazor.FlexGrid.Components.Renderers.FormInputs;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataAdapters.Visitors;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Http;
using Blazor.FlexGrid.Permission;
using Blazor.FlexGrid.State;
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
                services.TryAddScoped(typeof(ILazyDataSetLoader<>), typeof(NullLazyDataSetLoader<>));
                services.TryAddScoped(typeof(ILazyDataSetItemManipulator<>), typeof(NullLazyDataSetItemManipulator<>));
                services.TryAddScoped(typeof(ICreateItemHandle<,>), typeof(NullCreateItemHandler<,>));
                services.TryAddScoped(typeof(ILazyGroupableDataSetLoader<>), typeof(NullLazyGroupableDataSetLoader<>));
                services.AddScoped<FlexGridInterop>();
                services.AddScoped(typeof(LazyLoadedTableDataAdapter<>));
                RegisterRendererTreeBuildersScoped(services);
            }
            else
            {
                if (flexGridOptions.UseAuthorizationForHttpRequests)
                {
                    services.AddHttpClient<IHttpClientFactory, AuthorizationHttpClientFactory>(client =>
                    {
                        client.BaseAddress = new Uri(flexGridOptions.BaseServerAddress);
                    });
                }
                else
                {
                    services.AddSingleton<IHttpClientFactory, DefaultHttpClientFactory>();
                }

                services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
                services.AddSingleton(typeof(ILazyDataSetLoader<>), typeof(HttpLazyDataSetLoader<>));
                services.AddSingleton(typeof(ILazyDataSetItemManipulator<>), typeof(HttpLazyDataSetItemManipulator<>));
                services.AddSingleton(typeof(ICreateItemHandle<,>), typeof(HttpCreateItemHandler<,>));
                services.AddSingleton(typeof(ILazyGroupableDataSetLoader<>), typeof(HttpLazyGroupableDataSetLoader<>));
                services.AddSingleton<FlexGridInterop>();
                services.AddSingleton(typeof(LazyLoadedTableDataAdapter<>));
                RegisterRendererTreeBuilders(services);
            }

            services.AddMemoryCache();
            services.AddSingleton<IStateCache, MemoryStateCache>();
            services.TryAddSingleton<IAuthorizationService, NullAuthorizationService>();
            services.TryAddSingleton<ICurrentUserPermission>(new NullCurrentUserPermission());
            services.AddSingleton(typeof(MasterTableDataAdapterBuilder<>));
            services.AddSingleton(typeof(IGridConfigurationProvider), new GridConfigurationProvider(modelBuilder.Model));
            services.AddSingleton<IMasterDetailTableDataSetFactory, MasterDetailTableDataSetFactory>();
            services.AddSingleton<ConventionsSet>();
            services.AddSingleton<ITypePropertyAccessorCache, PropertyValueAccessorCache>();
            services.AddSingleton<IDetailDataAdapterVisitors, DetailDataAdapterVisitors>();
            services.AddSingleton<ITableDataAdapterProvider, RunTimeTableDataAdapterProvider>();
            RegisterFormInputBuilders(services);

            return services;
        }

        private static void RegisterFormInputBuilders(IServiceCollection services)
        {
            services.AddSingleton<IFormInputRendererBuilder, TextInputBuilder>();
            services.AddSingleton<IFormInputRendererBuilder, NumberInputBuilder>();
            services.AddSingleton<IFormInputRendererBuilder, SelectInputBuilder>();
            services.AddSingleton<IFormInputRendererBuilder, DateInputBuilder>();
            services.AddSingleton<IFormInputRendererBuilder, CheckBoxInputBuilder>();
            services.AddSingleton<IFormInputRendererTreeProvider, FormInputsRendererTreeProvider>();
            services.TryAddSingleton(typeof(IFormLayoutProvider<>), typeof(BasicFormLayoutProvider<>));
        }

        private static void RegisterRendererTreeBuilders(IServiceCollection services)
        {
            services.AddSingleton(typeof(CreateItemFormRenderer<>));
            services.AddSingleton(typeof(BlazorComponentColumnCollection<>));
            services.AddSingleton<ISpecialColumnFragmentsCollection, BlazorComponentColumnCollection>();
            services.AddSingleton<GridContextsFactory>();
            services.AddSingleton<EditInputRendererTree>();

            services.AddSingleton(typeof(IGridRendererTreeBuilder), provider =>
            {
                return CreateGridRenderer(provider);
            });
        }

        private static void RegisterRendererTreeBuildersScoped(IServiceCollection services)
        {
            services.AddSingleton(typeof(CreateItemFormRenderer<>));
            services.AddSingleton(typeof(BlazorComponentColumnCollection<>));
            services.AddSingleton<ISpecialColumnFragmentsCollection, BlazorComponentColumnCollection>();
            services.AddSingleton<GridContextsFactory>();
            services.AddSingleton<EditInputRendererTree>();

            services.AddScoped(typeof(IGridRendererTreeBuilder), provider =>
            {
                return CreateGridRenderer(provider);
            });
        }

        private static object CreateGridRenderer(IServiceProvider provider)
        {
            var gridRowRenderer = new GridRowRenderer()
                .AddRenderer(new GridCellMasterActionRenderer())
                .AddRenderer(new GridCellRenderer(provider.GetRequiredService<EditInputRendererTree>()))
                .AddRenderer(new GridTabControlRenderer(provider.GetRequiredService<ITableDataAdapterProvider>()), RendererType.AfterTag)
                .AddRenderer(new GridActionButtonsRenderer(provider.GetRequiredService<FlexGridInterop>()));

            var gridBodySimpleRenderer = new GridBodySimpleRenderer(provider.GetRequiredService<ILogger<GridBodySimpleRenderer>>())
                .AddRenderer(gridRowRenderer);

            var gridBodyGroupedRenderer = new GridBodyGroupedRenderer(
                    provider.GetRequiredService<ITableDataAdapterProvider>(),
                    provider.GetRequiredService<ILogger<GridBodyGroupedRenderer>>())
                .AddRenderer(gridRowRenderer);

            var gridBodyRenderer = new GridBodyRenderer(gridBodySimpleRenderer, gridBodyGroupedRenderer);
            var gridRenderer = new GridRenderer(provider.GetRequiredService<ILogger<GridRenderer>>())
                .AddRenderer(new GridHeaderRenderer(provider.GetRequiredService<FlexGridInterop>()))
                .AddRenderer(new GridEmptyItemsRenderer())
                .AddRenderer(gridBodyRenderer)
                .AddRenderer(new GridFooterRenderer(), RendererType.AfterTag)
                .AddRenderer(new DeleteModalRenderer(provider.GetRequiredService<FlexGridInterop>()), RendererType.AfterTag);

            return gridRenderer;
        }
    }
}
