using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.Builders;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.Demo.Shared;
using Blazor.FlexGrid.Tests.Mocks;
using Microsoft.AspNetCore.Blazor.RenderTree;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using Xunit;

namespace Blazor.FlexGrid.Tests
{
    public class RendererTests
    {
        [Fact]
        public void GridBodyRendererShouldRenderTBodyTag()
        {
            //Arrange
            var modelBuilder = new ModelBuilder();
            modelBuilder.Entity<WeatherForecast>()
                    .Property(p => p.Date)
                    .HasValueFormatter(d => d.ToShortDateString());

            var configurationProvider = new GridConfigurationProvider(modelBuilder.Model);
            var rendererContextFactory = new GridRendererContextFactory(configurationProvider);

            var source = Enumerable.Range(1, 20).Select(index =>
                        new WeatherForecast
                        {
                            Date = DateTime.Now.AddDays(index),
                            TemperatureC = 120,
                            Summary = "test"
                        }).AsQueryable();

            var tableDataSet = new TableDataSet<WeatherForecast>(source);
            tableDataSet.GoToPage(0);
            var rendererTreeBuilder = new RenderTreeBuilder(new TestRenderer());
            var rendererContext = rendererContextFactory.CreateRendererContext(tableDataSet, rendererTreeBuilder);
            var gridBodyRenderer = new GridBodyRenderer(NullLogger<GridBodyRenderer>.Instance);

            //Act
            gridBodyRenderer.Render(rendererContext);
        }
    }
}
