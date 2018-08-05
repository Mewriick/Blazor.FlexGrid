using Blazor.FlexGrid.Demo.Client.GridConfigurations;
using Blazor.FlexGrid.Demo.Serverside.App.Services;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.FlexGrid.Demo.Serverside.App
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Since Blazor is running on the server, we can use an application service
            // to read the forecast data.
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<CustomerService>();
            services.AddSingleton<OrderService>();
            services.AddFlexGridServerSide(cfg =>
            {
                cfg.ApplyConfiguration(new WeatherForecastGridConfiguration());
                cfg.ApplyConfiguration(new CustomerGridConfiguration());
                cfg.ApplyConfiguration(new OrderGridConfiguration());
            });
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
