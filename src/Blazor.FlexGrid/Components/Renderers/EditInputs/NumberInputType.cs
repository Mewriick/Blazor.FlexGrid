using Microsoft.AspNetCore.Blazor.Components;

namespace Blazor.FlexGrid.Components.Renderers.EditInputs
{
    public class NumberInputType : AbstractEditInputRenderer
    {
        public override void RenderInput(GridRendererContext gridRendererContext)
        {
            var localColumnName = gridRendererContext.ActualColumnName;
            var value = gridRendererContext.PropertyValueAccessor.GetValue(gridRendererContext.ActualItem, localColumnName);
            if (IsSupportedNumberType(value))
            {
                gridRendererContext.OpenElement("input");
                gridRendererContext.AddAttribute("type", "number");
                gridRendererContext.AddAttribute("value", BindMethods.GetValue(value));
                TryAddOnChangeHandler(gridRendererContext, localColumnName, value);
                gridRendererContext.CloseElement();
            }
            else
            {
                successor.RenderInput(gridRendererContext);
            }
        }

        private bool IsSupportedNumberType(object value)
            => value is int || value is long || value is decimal || value is double;

        private void TryAddOnChangeHandler(GridRendererContext gridRendererContext, string localColumnName, object value)
        {
            if (value is int intValue)
            {
                gridRendererContext.AddAttribute("onchange", BindMethods.SetValueHandler(delegate (int __value)
                    {
                        gridRendererContext
                            .TableDataSet
                            .EditItemProperty(localColumnName, __value);
                    }, intValue)
                );

            }
            else
            {
                AddLongValueHandlerIfValueIsLong(gridRendererContext, localColumnName, value);
            }
        }

        private void AddLongValueHandlerIfValueIsLong(GridRendererContext gridRendererContext, string localColumnName, object value)
        {
            if (value is long longValue)
            {
                gridRendererContext.AddAttribute("onchange", BindMethods.SetValueHandler(delegate (long __value)
                    {
                        gridRendererContext
                            .TableDataSet
                            .EditItemProperty(localColumnName, __value);
                    }, longValue)
                );
            }
            else
            {
                AddDecimalValueHandlerIfValueIsDecimal(gridRendererContext, localColumnName, value);
            }
        }

        private void AddDecimalValueHandlerIfValueIsDecimal(GridRendererContext gridRendererContext, string localColumnName, object value)
        {
            if (value is decimal decimalValue)
            {
                gridRendererContext.AddAttribute("onchange", BindMethods.SetValueHandler(delegate (decimal __value)
                    {
                        gridRendererContext
                            .TableDataSet
                            .EditItemProperty(localColumnName, __value);
                    }, decimalValue)
                );

            }
            else
            {
                AddDoubleValueHandlerIfValueIsDouble(gridRendererContext, localColumnName, value);
            }
        }

        private void AddDoubleValueHandlerIfValueIsDouble(GridRendererContext gridRendererContext, string localColumnName, object value)
        {
            if (value is double doubleValue)
            {
                gridRendererContext.AddAttribute("onchange", BindMethods.SetValueHandler(delegate (double __value)
                    {
                        gridRendererContext
                            .TableDataSet
                            .EditItemProperty(localColumnName, __value);
                    }, doubleValue)
                );
            }
        }
    }
}
