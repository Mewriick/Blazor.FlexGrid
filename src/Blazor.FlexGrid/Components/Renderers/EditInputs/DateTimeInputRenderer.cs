using Microsoft.AspNetCore.Components;
using System;

namespace Blazor.FlexGrid.Components.Renderers.EditInputs
{
    public class DateTimeInputRenderer : AbstractEditInputRenderer
    {
        public override void RenderInput(GridRendererContext gridRendererContext)
        {
            var localColumnName = gridRendererContext.ActualColumnName;
            var value = gridRendererContext.PropertyValueAccessor.GetValue(gridRendererContext.ActualItem, localColumnName);
            if (IsSupportedDateType(value))
            {
                var dateTimeValue = ConvertToDateTime(value);
                var dateValueContatinsTime = dateTimeValue.TimeOfDay.TotalSeconds != 0;
                var dateFormat = dateValueContatinsTime ? "yyyy-MM-dd'T'HH:mm:ss" : "yyyy-MM-dd";

                gridRendererContext.OpenElement(HtmlTagNames.Div, "edit-field-wrapper");
                gridRendererContext.OpenElement(HtmlTagNames.Input, "edit-text-field");
                gridRendererContext.AddAttribute(HtmlAttributes.Type, dateValueContatinsTime ? "datetime-local" : "date");
                gridRendererContext.AddAttribute(HtmlAttributes.Value, BindMethods.GetValue(dateTimeValue, dateFormat));
                gridRendererContext.AddAttribute(HtmlJSEvents.OnChange, BindMethods.SetValueHandler(delegate (DateTime __value)
                    {
                        gridRendererContext
                            .TableDataSet
                            .EditItemProperty(localColumnName, __value);
                    }, dateTimeValue, dateFormat)
                );

                gridRendererContext.CloseElement();
                gridRendererContext.CloseElement();
            }
            else
            {
                successor.RenderInput(gridRendererContext);
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
