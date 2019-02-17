using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.Builders;
using Blazor.FlexGrid.Demo.Shared;

namespace Blazor.FlexGrid.Demo.Client.GridConfigurations
{
    public class WeatherForecastGridConfiguration : IEntityTypeConfiguration<WeatherForecast>
    {
        public void Configure(EntityTypeBuilder<WeatherForecast> builder)
        {
            builder.AllowCreateItem();
            builder.AllowInlineEdit();

            builder.Property(e => e.Date)
                .HasCaption("Date")
                //.HasWritePermissionRestriction(perm => perm.IsInRole("TestRole1"))
                .HasValueFormatter(d => d.ToShortDateString());

            builder.Property(e => e.Summary)
                .HasCaption("MySummary")
                .HasOrder(1)
                //.HasValueFormatter(s => $"{s}!");
                .HasCompositeValueFormatter(f => $"{f.Summary} <button>{f.TemperatureC}</button> {f.TemperatureF}");

            builder.Property(e => e.TemperatureC)
                .IsSortable();

            builder.Property(e => e.TemperatureF)
                .IsSortable();
        }
    }
}
