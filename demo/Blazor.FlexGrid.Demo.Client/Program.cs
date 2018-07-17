using Blazor.FlexGrid.Demo.Client.GridConfigurations;
using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;

namespace Blazor.FlexGrid.Demo.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(services =>
            {
                services.AddFlexGrid(cfg =>
                {
                    cfg.ApplyConfiguration(new WeatherForecastGridConfiguration());
                    cfg.ApplyConfiguration(new CustomerGridConfiguration());
                });
            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
