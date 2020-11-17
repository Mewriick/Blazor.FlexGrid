using Blazor.FlexGrid.Demo.Client.GridConfigurations;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Demo.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault();
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp =>
                new System.Net.Http.HttpClient
                {
                    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
                });

            builder.Services.AddFlexGrid(
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
                    options.BaseServerAddress = builder.HostEnvironment.BaseAddress;
                }
            )
            .AddSingleton<ICurrentUserPermission, TestCurrentUserPermission>()
            .AddSingleton<IAuthorizationService, TestAuthorizationService>();

            await builder.Build().RunAsync();
        }
    }
}
