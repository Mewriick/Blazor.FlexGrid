using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.Builders;
using Blazor.FlexGrid.Demo.Shared;

namespace Blazor.FlexGrid.Demo.Client.GridConfigurations
{
    public class WeatherForecastGridConfiguration : IEntityTypeConfiguration<WeatherForecast>
    {
        public void Configure(EntityTypeBuilder<WeatherForecast> builder)
        {
            builder.Property(e => e.Date)
                .HasCaption("Date")
                .HasValueFormatter(d => d.ToShortDateString());

            builder.Property(e => e.Summary)
                .HasCaption("MySummary")
                .HasOrder(1)
                .HasValueFormatter(s => $"{s}!");

            builder.Property(e => e.TemperatureC)
                .IsSortable();

            builder.Property(e => e.TemperatureF)
                .IsSortable();
        }
    }
}
