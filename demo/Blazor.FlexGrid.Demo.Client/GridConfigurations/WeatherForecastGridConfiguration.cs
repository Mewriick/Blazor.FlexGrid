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
                .HasCaption("Datum")
                .HasValueFormatter(d => d.ToShortDateString());

            builder.Property(e => e.Summary)
                .HasCaption("MySummary")
                .HasValueFormatter(s => $"{s}!!");
        }
    }
}
