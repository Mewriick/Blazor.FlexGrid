using Blazor.Components.Demo.FlexGrid.GridConfigurations;
using Blazor.Components.Demo.FlexGrid.Services;
using Blazor.FlexGrid;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.Demo.Shared;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Blazor.Components.Demo.FlexGrid
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddNewtonsoftJson();

            services.AddServerSideBlazor();

            // Since Blazor is running on the server, we can use an application service
            // to read the forecast data.
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<CustomerService>();
            services.AddSingleton<OrderService>();
            services.AddFlexGridServerSide(
                cfg =>
                {
                    cfg.ApplyConfiguration(new WeatherForecastGridConfiguration());
                    cfg.ApplyConfiguration(new CustomerGridConfiguration());
                    cfg.ApplyConfiguration(new OrderGridConfiguration());
                }
            );

            services.AddScoped<ILazyDataSetLoader<Order>, OrderService>();

            services.AddScoped<ILazyDataSetLoader<WeatherForecast>, WeatherForecastService>();
            services.AddScoped<ILazyGroupableDataSetLoader<WeatherForecast>, WeatherForecastService>();
            services.AddScoped<ILazyDataSetItemManipulator<WeatherForecast>, WeatherForecastService>();

            services.AddSingleton<ICurrentUserPermission, TestCurrentUserPermission>();
            services.AddSingleton<StaticRepositoryCollections>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseFlexGrid(env.WebRootPath);

            app.UseRouting();
            app.UseEndpoints(routes =>
            {
                routes.MapRazorPages();
                routes.MapFallbackToPage("/_Host");
                routes.MapBlazorHub();
            });
        }
    }
}
