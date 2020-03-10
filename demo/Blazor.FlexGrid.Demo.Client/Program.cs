using Blazor.FlexGrid.Demo.Client.GridConfigurations;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Demo.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault();
            builder.RootComponents.Add<App>("app");

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
                }
            )
            .AddSingleton<ICurrentUserPermission, TestCurrentUserPermission>()
            .AddSingleton<IAuthorizationService, TestAuthorizationService>();

            await builder.Build().RunAsync();
        }
    }
}
