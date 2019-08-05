using Blazor.FlexGrid.Components;
using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.Builders;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.Demo.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;

namespace Blazor.Components.Demo.FlexGrid.GridConfigurations
{
    public class WeatherForecastGridConfiguration : IEntityTypeConfiguration<WeatherForecast>
    {
        public void Configure(EntityTypeBuilder<WeatherForecast> builder)
        {
            builder.AllowCreateItem<WeatherForecastCreateModel, WeatherForecast>(conf =>
            {
                conf.CreatePermissionRestriction = p => p.IsInRole("TestRole");
                conf.CreateUri = "/api/SampleData/WeatherForecast";
            });

            builder.AllowInlineEdit();

            builder.Property(e => e.Date)
                .HasCaption("Date")
                //.HasWritePermissionRestriction(perm => perm.IsInRole("TestRole1"))
                .HasValueFormatter(d => d.ToShortDateString());

            builder.Property(e => e.TemperatureC)
                .IsFilterable()
                .IsSortable();

            builder.Property(e => e.TemperatureF)
                .IsFilterable()
                .IsSortable();

            builder.Property(e => e.RainyDay)
                .IsFilterable();

            builder.EnableGrouping(options =>
            {
                options.GroupPageSize = 15;
            });


            /* Func<EditColumnContext, RenderFragment<WeatherForecast>> weatherSummaryEdit =
                 context =>
             {
                 RenderFragment<WeatherForecast> summeryEdit = (WeatherForecast weather) => delegate (RenderTreeBuilder rendererTreeBuilder)
                 {
                     var internalBuilder = new BlazorRendererTreeBuilder(rendererTreeBuilder);
                     internalBuilder
                         .OpenElement(HtmlTagNames.Div, "edit-field-wrapper")
                         .OpenElement(HtmlTagNames.Input, "edit-text-field")
                         .AddAttribute(HtmlAttributes.Type, "text")
                         .AddAttribute(HtmlAttributes.Value, weather.Summary)
                         .AddAttribute(HtmlJSEvents.OnChange,
                             BindMethods.SetValueHandler(delegate (string __value)
                             {
                                 context.NotifyValueHasChanged($"{__value}_CustomEdit");
                             }, weather.Summary?.ToString() ?? string.Empty)
                         )
                         .CloseElement()
                         .CloseElement();
                 };

                 return summeryEdit;
             };
             */
            builder.Property(e => e.Summary)
                .HasCaption("MySummary")
                .HasOrder(1)
                //.HasValueFormatter(s => $"{s}!");
                .HasCompositeValueFormatter(f => $"{f.Summary} <button>{f.TemperatureC}</button> {f.TemperatureF}");
                //.HasBlazorEditComponent(weatherSummaryEdit);
        }
    }
}
