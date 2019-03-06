using Blazor.FlexGrid.Components.Renderers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.FlexGrid.Components
{
    public class ValidationError : ComponentBase
    {
        [Inject]
        private IRendererTreeBuilder RendererTreeBuilder { get; set; }

        [Parameter]
        protected IEnumerable<Validation.ValidationResult> ValidationErrors { get; set; }

        [Parameter]
        protected string PropertyName { get; set; }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            var propertyErrors = ValidationErrors.Where(ve => ve.MemberName.Equals(PropertyName));
            if (!propertyErrors.Any())
            {
                return;
            }

            RendererTreeBuilder
                .OpenElement(HtmlTagNames.Div)
                .OpenElement(HtmlTagNames.Ul);

            foreach (var error in propertyErrors)
            {
                RendererTreeBuilder
                    .OpenElement(HtmlTagNames.Li)
                    .AddContent(error.Message)
                    .CloseElement();
            }

            RendererTreeBuilder
                .CloseElement()
                .CloseElement();
        }
    }
}
