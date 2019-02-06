using Blazor.FlexGrid.Demo.Client.GridConfigurations;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.FlexGrid.Demo.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFlexGrid(
                cfg =>
                {
                    cfg.ApplyConfiguration(new WeatherForecastGridConfiguration());
                    cfg.ApplyConfiguration(new CustomerGridConfiguration());
                    cfg.ApplyConfiguration(new OrderGridConfiguration());
                },
                options =>
                {
                    options.IsServerSideBlazorApp = false;
                    options.UseAuthorizationForHttpRequests = true;
                }
            );

            services.AddSingleton<ICurrentUserPermission, TestCurrentUserPermission>();
            services.AddSingleton<IAuthorizationService, TestAuthorizationService>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
