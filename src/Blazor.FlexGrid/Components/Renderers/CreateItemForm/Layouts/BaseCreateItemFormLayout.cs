using Blazor.FlexGrid.Components.Renderers.FormInputs;
using System;
using System.Reflection;


namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm.Layouts
{
    public abstract class BaseCreateItemFormLayout<TModel> : IFormLayout<TModel> where TModel : class
    {
        public abstract Action<IRendererTreeBuilder> BuildBodyRendererTree(
            CreateItemRendererContext<TModel> createItemRendererContext,
            IFormInputRendererTreeProvider formInputRendererTreeProvider);


        public virtual Action<IRendererTreeBuilder> BuildFooterRendererTree(
            CreateItemRendererContext<TModel> createItemRendererContext)
        {
            return builder =>
            {
                builder
                    .OpenElement(HtmlTagNames.Div, createItemRendererContext.CreateFormCssClasses.ModalFooter)
                    .OpenElement(HtmlTagNames.Input, createItemRendererContext.CreateFormCssClasses.SubmitButton)
                    .AddAttribute(HtmlAttributes.Type, HtmlTagNames.Submit)
                    .AddAttribute(HtmlAttributes.Value, "Save")
                    .CloseElement()
                    .CloseElement();
            };
        }

        public Action<IRendererTreeBuilder> BuildFormFieldRendererTree(
            PropertyInfo field,
            CreateItemRendererContext<TModel> createItemRendererContext,
            IFormInputRendererTreeProvider formInputRendererTreeProvider)
        {
            createItemRendererContext.ActualColumnName = field.Name;
            var inputBuilder = formInputRendererTreeProvider.GetFormInputRendererTreeBuilder(field.GetMemberType());

            return builder =>
            {
                BuildFieldRendererTree(field, createItemRendererContext, inputBuilder)?.Invoke(builder);
            };
        }

        public virtual Action<IRendererTreeBuilder> BuildFieldRendererTree(
            PropertyInfo field,
            CreateItemRendererContext<TModel> createItemRendererContext,
            IFormInputRendererBuilder formInputRendererBuilder)
        {
            return builder =>
            {
                builder
                    .OpenElement(HtmlTagNames.Div, "form-group")
                    .OpenElement(HtmlTagNames.Label, createItemRendererContext.CreateFormCssClasses.FieldName)
                    .AddContent(field.Name)
                    .CloseElement();

                formInputRendererBuilder.BuildRendererTree(createItemRendererContext, field)?.Invoke(builder);

                builder.CloseElement();
            };
        }
    }
}
