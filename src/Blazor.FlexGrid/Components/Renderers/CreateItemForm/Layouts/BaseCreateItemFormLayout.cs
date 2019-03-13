using Blazor.FlexGrid.Components.Renderers.FormInputs;
using System;
using System.Reflection;


namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm.Layouts
{
    public abstract class BaseCreateItemFormLayout<TItem> : ICreateFormLayout<TItem> where TItem : class
    {
        public abstract Action<IRendererTreeBuilder> BuildBodyRendererTree(
            CreateItemRendererContext<TItem> createItemRendererContext,
            IFormInputRendererTreeProvider formInputRendererTreeProvider);


        public virtual Action<IRendererTreeBuilder> BuildFooterRendererTree(
            CreateItemRendererContext<TItem> createItemRendererContext)
        {
            return builder =>
            {
                builder
                    .OpenElement(HtmlTagNames.Div, "modal-footer")
                    .OpenElement(HtmlTagNames.Input, "btn btn-primary")
                    .AddAttribute(HtmlAttributes.Type, HtmlTagNames.Submit)
                    .AddAttribute(HtmlAttributes.Value, "Save")
                    .CloseElement()
                    .CloseElement();
            };
        }

        public Action<IRendererTreeBuilder> BuildFormFieldRendererTree(
            PropertyInfo field,
            CreateItemRendererContext<TItem> createItemRendererContext,
            IFormInputRendererTreeProvider formInputRendererTreeProvider)
        {
            createItemRendererContext.ActualColumnName = field.Name;

            return builder =>
            {
                BuilFieldRendererTree(field, createItemRendererContext, formInputRendererTreeProvider)?.Invoke(builder);
            };
        }

        public virtual Action<IRendererTreeBuilder> BuilFieldRendererTree(
            PropertyInfo field,
            CreateItemRendererContext<TItem> createItemRendererContext,
            IFormInputRendererTreeProvider formInputRendererTreeProvider)
        {
            var inputBuilder = formInputRendererTreeProvider.GetFormInputRendererTreeBuilder(field.GetMemberType());

            return builder =>
            {
                builder
                    .OpenElement(HtmlTagNames.Div, "form-group")
                    //.OpenElement(HtmlTagNames.Div, "form-edit-field")
                    .OpenElement(HtmlTagNames.Label, "edit-field-name")
                    .AddContent(field.Name)
                    .CloseElement();

                inputBuilder.BuildRendererTree(createItemRendererContext, field)?.Invoke(builder);

                builder
                    //.CloseElement()
                    .CloseElement();
            };
        }
    }
}
