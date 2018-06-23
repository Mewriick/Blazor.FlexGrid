using Blazor.FlexGrid.Components.Configuration.Builders;
using Blazor.FlexGrid.Demo.Shared;
using Xunit;

namespace Blazor.FlexGrid.Tests
{
    public class ModelBuilderTests
    {
        [Fact]
        public void ChainingMetaDataBuilders()
        {
            var modelBuilder = new ModelBuilder();
            modelBuilder
                .Entity<WeatherForecast>()
                    .Property(p => p.Summary)
                    .HasCaption("Summary!")
                    .HasOrder(5)
                    .IsVisible(true);

            modelBuilder
                .Entity<WeatherForecast>()
                    .Property(p => p.Date)
                    .HasValueFormatter(d => d.ToShortDateString());

            var model = modelBuilder.Model;
        }
    }
}
