using Microsoft.AspNetCore.Components;
using System;

namespace Blazor.FlexGrid.Components.Renderers.EditInputs
{
    public class DateTimeInputRenderer : AbstractEditInputRenderer
    {
        public override void BuildInputRendererTree(IRendererTreeBuilder rendererTreeBuilder, IActualItemContext<object> actualItemContext, Action<string, object> onChangeAction)
        {
            var localColumnName = actualItemContext.ActualColumnName;
            var value = actualItemContext.GetActualItemColumnValue(localColumnName);
            if (IsSupportedDateType(value))
            {
                var dateTimeValue = ConvertToDateTime(value);
                var dateValueContatinsTime = dateTimeValue.TimeOfDay.TotalSeconds != 0;
                var dateFormat = dateValueContatinsTime ? "yyyy-MM-dd'T'HH:mm:ss" : "yyyy-MM-dd";

                rendererTreeBuilder
                    .OpenElement(HtmlTagNames.Div, "edit-field-wrapper")
                    .OpenElement(HtmlTagNames.Input, "edit-text-field")
                    .AddAttribute(HtmlAttributes.Type, dateValueContatinsTime ? "datetime-local" : "date")
                    .AddAttribute(HtmlAttributes.Value, BindConverter.FormatValue(dateTimeValue, dateFormat))
                    .AddAttribute(HtmlJSEvents.OnChange, EventCallback.Factory.Create(this,
                        (UIChangeEventArgs e) =>
                        {
                            onChangeAction?.Invoke(localColumnName, BindConverterExtensions.ConvertTo(e.Value, DateTime.MinValue));
                        }))
                    .CloseElement()
                    .CloseElement();
            }
            else
            {
                successor.BuildInputRendererTree(rendererTreeBuilder, actualItemContext, onChangeAction);
            }
        }

        private bool IsSupportedDateType(object value)
            => value is DateTime || value is DateTimeOffset;

        private DateTime ConvertToDateTime(object value)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                return dateTimeOffset.DateTime;
            }

            if (value is DateTime dateTime)
            {
                return dateTime;
            }

            return DateTime.Now;
        }
    }
}
