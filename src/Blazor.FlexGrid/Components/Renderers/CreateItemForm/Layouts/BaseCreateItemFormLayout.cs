using Blazor.FlexGrid.Components.Renderers.EditInputs;
using System;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm.Layouts
{
    public abstract class BaseCreateItemFormLayout<TItem> : ICreateFormLayout<TItem> where TItem : class
    {
        public abstract Action<IRendererTreeBuilder> BuildBodyRendererTree(
            CreateItemRendererContext<TItem> createItemRendererContext,
            AbstractEditInputRenderer editInputRenderer);


        public virtual Action<IRendererTreeBuilder> BuildFooterRendererTree(
            CreateItemRendererContext<TItem> createItemRendererContext)
        {
            return builder =>
            {
                builder
                    .OpenElement(HtmlTagNames.Div, "modal-footer")
                    .OpenElement(HtmlTagNames.Button, "btn btn-primary")
                    .AddContent("Save")
                    .CloseElement()
                    .CloseElement();
            };
        }

        public Action<IRendererTreeBuilder> BuildFormFieldRendererTree(
            PropertyInfo field,
            CreateItemRendererContext<TItem> createItemRendererContext,
            AbstractEditInputRenderer editInputRenderer)
        {
            createItemRendererContext.ActualColumnName = field.Name;

            return BuilFieldRendererTree(field, createItemRendererContext, editInputRenderer);
        }

        public virtual Action<IRendererTreeBuilder> BuilFieldRendererTree(
            PropertyInfo field,
            CreateItemRendererContext<TItem> createItemRendererContext,
            AbstractEditInputRenderer editInputRenderer)
        {
            return builder =>
            {
                builder
                    .OpenElement(HtmlTagNames.Div, "form-group")
                    .OpenElement(HtmlTagNames.Div, "form-edit-field")
                    .OpenElement(HtmlTagNames.Label, "edit-field-name")
                    .AddContent(field.Name)
                    .CloseElement();

                editInputRenderer.BuildInputRendererTree(builder, createItemRendererContext, createItemRendererContext.SetActulItemColumnValue);

                builder
                    .CloseElement()
                    .CloseElement();
            };
        }
    }
}
