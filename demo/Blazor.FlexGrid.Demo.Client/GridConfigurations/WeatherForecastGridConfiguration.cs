using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.Builders;
using Blazor.FlexGrid.Demo.Shared;

namespace Blazor.FlexGrid.Demo.Client.GridConfigurations
{
    public class WeatherForecastGridConfiguration : IEntityTypeConfiguration<WeatherForecast>
    {
        public void Configure(EntityTypeBuilder<WeatherForecast> builder)
        {
            builder.AllowInlineEdit(conf =>
            {
                conf.AllowDeleting = true;
                conf.DeletePermissionRestriction = perm => perm.IsInRole("TestRole");
            });

            builder.Property(e => e.Id)
                .IsVisible(false);

            builder.Property(e => e.Date)
                .HasCaption("Date")
                .HasValueFormatter(d => d.ToShortDateString());

            builder.Property(e => e.Summary)
                .HasCaption("MySummary")
                .HasOrder(1)
                //.HasValueFormatter(s => $"<button>{s}!</button>");
                .HasCompositeValueFormatter(f => $"{f.Summary} - {f.TemperatureC} - {f.TemperatureF}");

            builder.Property(e => e.TemperatureC)
                .IsSortable();

            builder.Property(e => e.TemperatureF)
                .IsSortable();
        }
    }
}
