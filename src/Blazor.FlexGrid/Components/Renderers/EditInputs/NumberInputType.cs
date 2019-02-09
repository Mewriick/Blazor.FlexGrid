using Microsoft.AspNetCore.Components;
using System;

namespace Blazor.FlexGrid.Components.Renderers.EditInputs
{
    public class NumberInputType : AbstractEditInputRenderer
    {
        public override void RenderInput(IRendererTreeBuilder rendererTreeBuilder, IActualItemContext actualItemContext, Action<string, object> onChangeAction)
        {
            var localColumnName = actualItemContext.ActualColumnName;
            var value = actualItemContext.GetActualItemColumnValue(localColumnName);
            if (IsSupportedNumberType(value))
            {
                rendererTreeBuilder
                    .OpenElement(HtmlTagNames.Div, "edit-field-wrapper")
                    .OpenElement(HtmlTagNames.Input, "edit-text-field")
                    .AddAttribute(HtmlAttributes.Type, "number")
                    .AddAttribute(HtmlAttributes.Value, BindMethods.GetValue(value));

                TryAddOnChangeHandler(rendererTreeBuilder, onChangeAction, localColumnName, value);

                rendererTreeBuilder
                    .CloseElement()
                    .CloseElement();
            }
            else
            {
                successor.RenderInput(rendererTreeBuilder, actualItemContext, onChangeAction);
            }
        }

        private bool IsSupportedNumberType(object value)
            => value is int || value is long || value is decimal || value is double;

        private void TryAddOnChangeHandler(IRendererTreeBuilder rendererTreeBuilder, Action<string, object> onChangeAction, string localColumnName, object value)
        {
            if (value is int intValue)
            {
                rendererTreeBuilder.AddAttribute(HtmlJSEvents.OnChange, BindMethods.SetValueHandler(delegate (int __value)
                    {
                        onChangeAction?.Invoke(localColumnName, __value);
                    }, intValue)
                );

            }
            else
            {
                AddLongValueHandlerIfValueIsLong(rendererTreeBuilder, onChangeAction, localColumnName, value);
            }
        }

        private void AddLongValueHandlerIfValueIsLong(IRendererTreeBuilder rendererTreeBuilder, Action<string, object> onChangeAction, string localColumnName, object value)
        {
            if (value is long longValue)
            {
                rendererTreeBuilder.AddAttribute(HtmlJSEvents.OnChange, BindMethods.SetValueHandler(delegate (long __value)
                    {
                        onChangeAction?.Invoke(localColumnName, __value);
                    }, longValue)
                );
            }
            else
            {
                AddDecimalValueHandlerIfValueIsDecimal(rendererTreeBuilder, onChangeAction, localColumnName, value);
            }
        }

        private void AddDecimalValueHandlerIfValueIsDecimal(IRendererTreeBuilder rendererTreeBuilder, Action<string, object> onChangeAction, string localColumnName, object value)
        {
            if (value is decimal decimalValue)
            {
                rendererTreeBuilder.AddAttribute(HtmlJSEvents.OnChange, BindMethods.SetValueHandler(delegate (decimal __value)
                    {
                        onChangeAction?.Invoke(localColumnName, __value);
                    }, decimalValue)
                );

            }
            else
            {
                AddDoubleValueHandlerIfValueIsDouble(rendererTreeBuilder, onChangeAction, localColumnName, value);
            }
        }

        private void AddDoubleValueHandlerIfValueIsDouble(IRendererTreeBuilder rendererTreeBuilder, Action<string, object> onChangeAction, string localColumnName, object value)
        {
            if (value is double doubleValue)
            {
                rendererTreeBuilder.AddAttribute(HtmlJSEvents.OnChange, BindMethods.SetValueHandler(delegate (double __value)
                    {
                        onChangeAction?.Invoke(localColumnName, __value);
                    }, doubleValue)
                );
            }
        }
    }
}
