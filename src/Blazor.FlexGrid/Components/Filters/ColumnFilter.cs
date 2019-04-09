using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.Filters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Blazor.FlexGrid.Components.Filters
{
    public class ColumnFilter<TValue> : ComponentBase
    {
        private static Dictionary<string, ColumnFilterState> stateCache = new Dictionary<string, ColumnFilterState>();

        private const string WrapperCssClass = "filter-wrapper";
        private const string WrapperCssCheckboxClass = "filter-wrapper-checkbox";

        private const FilterOperation StringFilterOperations = FilterOperation.Contains | FilterOperation.EndsWith | FilterOperation.StartsWith | FilterOperation.NotEqual | FilterOperation.Equal;
        private const FilterOperation NumberFilterOperations = FilterOperation.GreaterThan | FilterOperation.GreaterThanOrEqual | FilterOperation.LessThan |
            FilterOperation.LessThanOrEqual | FilterOperation.Equal | FilterOperation.NotEqual;

        delegate bool Parser(string value, out TValue result);
        private static Parser parser;
        private static FilterOperation allowedFilterOperations;

        private FilterOperation selectedFilterOperation;
        private TValue actualFilterValue;
        private bool filterDefinitionOpened = false;
        private bool filterIsApplied = false;
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
            else if (targetType == typeof(bool))
            {
                parser = TryParseBool;
            }
            else
            {
                throw new InvalidOperationException($"The type '{targetType}' is not a supported type.");
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            LoadStateIfExists();

            var rendererBuilder = new BlazorRendererTreeBuilder(builder);

            rendererBuilder
                .OpenElement(HtmlTagNames.Button, filterIsApplied ? "action-button action-button-small action-button-filter-active" : "action-button action-button-small")
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
                .CloseElement();


            rendererBuilder.OpenElement(HtmlTagNames.Div,
                filterDefinitionOpened
                ? parser != TryParseBool
                        ? $"{WrapperCssClass} {WrapperCssClass}-open"
                        : $"{WrapperCssClass} {WrapperCssClass}-open {WrapperCssCheckboxClass}"
                : $"{WrapperCssClass}");

            if (parser == TryParseBool)
            {
                BuildRendererTreeForCheckbox(rendererBuilder);
            }
            else
            {
                BuildRendererTreeForFilterOperations(rendererBuilder);
                BuildRendererTreeForInputs(rendererBuilder);
            }

            if (parser != TryParseBool)
            {
                rendererBuilder.OpenElement(HtmlTagNames.Div, "filter-buttons");
                rendererBuilder.OpenElement(HtmlTagNames.Button, "btn btn-light filter-buttons-clear")
                    .AddOnClickEvent(() =>
                        BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                        {
                            ClearFilter();
                        })
                    )
                    .AddContent("Clear")
                    .CloseElement()
                    .CloseElement();
            }

            rendererBuilder.CloseElement();
        }

        protected override void OnInit()
        {
            base.OnInit();

            filterContext = CascadeFlexGridContext.FilterContext;
        }

        private void FilterValueChanged(string value)
        {
            parser(value, out actualFilterValue);
            AddFilterDefinition();
            filterDefinitionOpened = false;
        }

        private void FilterBoolValueChanged(bool value)
        {
            actualFilterValue = (TValue)(object)value;
            selectedFilterOperation = FilterOperation.Equal;
            AddFilterDefinition();
        }

        private void AddFilterDefinition()
        {
            filterContext.AddOrUpdateFilterDefinition(new ExpressionFilterDefinition(ColumnName, selectedFilterOperation, actualFilterValue));
            filterIsApplied = true;
            CacheActualState();
        }

        private void ClearFilter()
        {
            filterContext.RemoveFilter(ColumnName);
            filterIsApplied = false;
            filterDefinitionOpened = false;
            stateCache.Remove(ColumnName);
        }

        private void LoadStateIfExists()
        {
            if (stateCache.TryGetValue(ColumnName, out var columnFilterState))
            {
                selectedFilterOperation = columnFilterState.FilterOperation;
                actualFilterValue = (TValue)columnFilterState.FilterValue;
                filterIsApplied = true;
            }
        }

        private void CacheActualState()
        {
            var filterState = new ColumnFilterState(actualFilterValue, selectedFilterOperation);
            if (!stateCache.ContainsKey(ColumnName))
            {
                stateCache.Add(ColumnName, filterState);
            }
            else
            {
                stateCache[ColumnName] = filterState;
            }
        }

        private void BuildRendererTreeForInputs(BlazorRendererTreeBuilder rendererBuilder)
        {
            if (parser == TryParseDateTime || parser == TryParseDateTimeOffset)
            {
                rendererBuilder
                    .OpenElement(HtmlTagNames.Input, "edit-text-field edit-date-field-filter")
                    .AddAttribute(HtmlAttributes.Type, HtmlAttributes.TypeDate)
                    .AddAttribute(HtmlAttributes.Value, BindMethods.GetValue(FormatDateAsString(actualFilterValue)))
                    .AddAttribute(HtmlJSEvents.OnChange, BindMethods.SetValueHandler(delegate (string __value)
                    {
                        FilterValueChanged(__value);
                    }, FormatDateAsString(actualFilterValue)))
                    .CloseElement();

                return;
            }

            rendererBuilder
                .OpenElement(HtmlTagNames.Input, "edit-text-field edit-text-field-filter")
                .AddAttribute(HtmlAttributes.Value, BindMethods.GetValue(filterIsApplied ? actualFilterValue.ToString() : string.Empty))
                .AddAttribute(HtmlJSEvents.OnChange, BindMethods.SetValueHandler(delegate (string __value)
                {
                    FilterValueChanged(__value);
                }, actualFilterValue?.ToString() ?? string.Empty))
                .CloseElement();
        }

        private void BuildRendererTreeForCheckbox(BlazorRendererTreeBuilder rendererBuilder)
        {
            rendererBuilder
                .OpenElement(HtmlTagNames.Label, "switch")
                .OpenElement(HtmlTagNames.Input)
                .AddAttribute(HtmlAttributes.Type, HtmlAttributes.Checkbox)
                .AddAttribute(HtmlAttributes.Value, BindMethods.GetValue(actualFilterValue))
                .AddAttribute(HtmlJSEvents.OnChange, BindMethods.SetValueHandler(delegate (bool __value)
                {
                    FilterBoolValueChanged(__value);
                }, (bool)(object)actualFilterValue))
                .CloseElement()
                .OpenElement(HtmlTagNames.Span, "slider round")
                .CloseElement()
                .CloseElement();
        }

        private void BuildRendererTreeForFilterOperations(BlazorRendererTreeBuilder rendererBuilder)
        {
            rendererBuilder
                    .OpenElement(HtmlTagNames.Select)
                    .AddAttribute(HtmlJSEvents.OnChange, BindMethods.SetValueHandler(delegate (int __value)
                    {
                        selectedFilterOperation = (FilterOperation)__value;
                        if (filterIsApplied)
                        {
                            filterContext.AddOrUpdateFilterDefinition(new ExpressionFilterDefinition(ColumnName, selectedFilterOperation, actualFilterValue));
                        }

                    }, (int)selectedFilterOperation));

            foreach (var enumValue in Enum.GetValues(typeof(FilterOperation)))
            {
                var filterOperation = (FilterOperation)enumValue;

                if (!allowedFilterOperations.HasFlag(filterOperation)
                    || filterOperation == FilterOperation.None)
                {
                    continue;
                }

                selectedFilterOperation = selectedFilterOperation == FilterOperation.None
                    ? filterOperation
                    : selectedFilterOperation;

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

        private string FormatDateAsString(TValue value)
        {
            switch (value)
            {
                case DateTime dateTimeValue:
                    return dateTimeValue.ToString(FlexGridContext.DateFormat);
                case DateTimeOffset dateTimeOffsetValue:
                    return dateTimeOffsetValue.ToString(FlexGridContext.DateFormat);
                default:
                    return string.Empty;
            }
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

        static bool TryParseBool(string value, out TValue result)
        {
            var success = bool.TryParse(value, out var parsedValue);
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

    internal class ColumnFilterState
    {
        public object FilterValue { get; }

        public FilterOperation FilterOperation { get; }

        public ColumnFilterState(object filterValue, FilterOperation filterOperation)
        {
            FilterValue = filterValue;
            FilterOperation = filterOperation;
        }
    }
}
