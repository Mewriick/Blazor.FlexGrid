using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.Filters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Globalization;

namespace Blazor.FlexGrid.Components.Filters
{
    public class ColumnFilter<TValue> : ComponentBase
    {
        private const FilterOperation StringFilterOperations = FilterOperation.Contains | FilterOperation.EndsWith | FilterOperation.StartsWith | FilterOperation.NotEqual | FilterOperation.Equal;
        private const FilterOperation NumberFilterOperations = FilterOperation.GreaterThan | FilterOperation.GreaterThanOrEqual | FilterOperation.LessThan |
            FilterOperation.LessThanOrEqual | FilterOperation.Equal | FilterOperation.NotEqual;

        delegate bool Parser(string value, out TValue result);
        private static Parser parser;
        private static FilterOperation allowedFilterOperations;

        private FilterOperation selectedFilterOperation;
        private bool filterDefinitionOpened = false;
        private FilterContext filterContext;

        [CascadingParameter] FlexGridContext CascadeFlexGridContext { get; set; }

        [Parameter] string ColumnName { get; set; }

        static ColumnFilter()
        {
            var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

            if (targetType == typeof(string))
            {
                parser = TryParseString;
                allowedFilterOperations = StringFilterOperations;
            }
            else if (targetType == typeof(int))
            {
                parser = TryParseInt;
                allowedFilterOperations = NumberFilterOperations;
            }
            else if (targetType == typeof(long))
            {
                parser = TryParseLong;
                allowedFilterOperations = NumberFilterOperations;
            }
            else if (targetType == typeof(float))
            {
                parser = TryParseFloat;
                allowedFilterOperations = NumberFilterOperations;
            }
            else if (targetType == typeof(double))
            {
                parser = TryParseDouble;
                allowedFilterOperations = NumberFilterOperations;
            }
            else if (targetType == typeof(decimal))
            {
                parser = TryParseDecimal;
                allowedFilterOperations = NumberFilterOperations;
            }
            else if (targetType == typeof(DateTime))
            {
                parser = TryParseDateTime;
                allowedFilterOperations = NumberFilterOperations;
            }
            else if (targetType == typeof(DateTimeOffset))
            {
                parser = TryParseDateTimeOffset;
                allowedFilterOperations = NumberFilterOperations;
            }
            else
            {
                throw new InvalidOperationException($"The type '{targetType}' is not a supported type.");
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            var rendererBuilder = new BlazorRendererTreeBuilder(builder);

            rendererBuilder
                .OpenElement(HtmlTagNames.Button, "action-button action-button-small")
                .AddAttribute(HtmlJSEvents.OnClick,
                    BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                    {
                        filterDefinitionOpened = !filterDefinitionOpened;
                    })
                 )
                .OpenElement(HtmlTagNames.Span)
                .OpenElement(HtmlTagNames.I, "fas fa-filter")
                .CloseElement()
                .CloseElement()
                .CloseElement()
                .OpenElement(HtmlTagNames.Div, filterDefinitionOpened ? "filter-wrapper-open" : "filter-wrapper");

            BuildRendererTreeForFilterOperations(rendererBuilder);

            rendererBuilder
                .OpenElement(HtmlTagNames.Input)
                .AddAttribute(HtmlAttributes.Value, BindMethods.GetValue(string.Empty))
                .AddAttribute(HtmlJSEvents.OnChange, BindMethods.SetValueHandler(delegate (string __value)
                {
                    parser(__value, out var parsedValue);
                    filterContext.AddOrUpdateFilterDefinition(new ExpressionFilterDefinition(ColumnName, selectedFilterOperation, parsedValue));
                }, string.Empty))
                .CloseElement()
                .OpenElement(HtmlTagNames.Button)
                .AddOnClickEvent(() =>
                    BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                    {
                        filterContext.RemoveFilter(ColumnName);
                    })
                )
                .CloseElement()
                .CloseElement();
        }

        protected override void OnInit()
        {
            base.OnInit();

            filterContext = CascadeFlexGridContext.FilterContext;
        }

        private void BuildRendererTreeForFilterOperations(BlazorRendererTreeBuilder rendererBuilder)
        {
            rendererBuilder
                    .OpenElement(HtmlTagNames.Select)
                    .AddAttribute(HtmlJSEvents.OnChange, BindMethods.SetValueHandler(delegate (int __value)
                    {
                        selectedFilterOperation = (FilterOperation)__value;
                    }, (int)selectedFilterOperation));

            foreach (var enumValue in Enum.GetValues(typeof(FilterOperation)))
            {
                if (!allowedFilterOperations.HasFlag((FilterOperation)enumValue))
                {
                    continue;
                }

                var enumStringValue = enumValue.ToString();
                rendererBuilder.OpenElement(HtmlTagNames.Option);
                if (enumStringValue == selectedFilterOperation.ToString())
                {
                    rendererBuilder.AddAttribute(HtmlAttributes.Selected, true);
                }

                rendererBuilder
                    .AddAttribute(HtmlAttributes.Value, (int)enumValue)
                    .AddContent(enumStringValue)
                    .CloseElement();
            }

            rendererBuilder.CloseElement();
        }

        static bool TryParseString(string value, out TValue result)
        {
            result = (TValue)(object)value;

            return true;
        }

        static bool TryParseInt(string value, out TValue result)
        {
            var success = int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue);
            if (success)
            {
                result = (TValue)(object)parsedValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        static bool TryParseLong(string value, out TValue result)
        {
            var success = long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue);
            if (success)
            {
                result = (TValue)(object)parsedValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        static bool TryParseFloat(string value, out TValue result)
        {
            var success = float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue);
            if (success)
            {
                result = (TValue)(object)parsedValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        static bool TryParseDouble(string value, out TValue result)
        {
            var success = double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue);
            if (success)
            {
                result = (TValue)(object)parsedValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        static bool TryParseDecimal(string value, out TValue result)
        {
            var success = decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue);
            if (success)
            {
                result = (TValue)(object)parsedValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        static bool TryParseDateTime(string value, out TValue result)
        {
            var success = DateTime.TryParse(value, out var parsedValue);
            if (success)
            {
                result = (TValue)(object)parsedValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        static bool TryParseDateTimeOffset(string value, out TValue result)
        {
            var success = DateTimeOffset.TryParse(value, out var parsedValue);
            if (success)
            {
                result = (TValue)(object)parsedValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
    }
}
