using Blazor.FlexGrid.Demo.Client.GridConfigurations;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.FlexGrid.Demo.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFlexGrid(cfg =>
            {
                cfg.ApplyConfiguration(new WeatherForecastGridConfiguration());
                cfg.ApplyConfiguration(new CustomerGridConfiguration());
                cfg.ApplyConfiguration(new OrderGridConfiguration());
            });

            services.AddSingleton<ICurrentUserPermission, TestCurrentUserPermission>();

        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
