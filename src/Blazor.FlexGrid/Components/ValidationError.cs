using Blazor.FlexGrid.Components.Renderers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.FlexGrid.Components
{
    public class ValidationError : ComponentBase
    {
        [Parameter]
        protected IEnumerable<Validation.ValidationResult> ValidationErrors { get; set; }

        [Parameter]
        protected string PropertyName { get; set; }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var rendererTreeBuilder = new BlazorRendererTreeBuilder(builder);

            base.BuildRenderTree(builder);
            var propertyErrors = ValidationErrors.Where(ve => ve.MemberName.Equals(PropertyName));
            if (!propertyErrors.Any())
            {
                return;
            }

            rendererTreeBuilder
                .OpenElement(HtmlTagNames.Div)
                .OpenElement(HtmlTagNames.Ul);

            foreach (var error in propertyErrors)
            {
                rendererTreeBuilder
                    .OpenElement(HtmlTagNames.Li)
                    .AddContent(error.Message)
                    .CloseElement();
            }

            rendererTreeBuilder
                .CloseElement()
                .CloseElement();
        }
    }
}
