using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.FlexGrid.Components.Renderers.FormInputs
{
    public class FormInputsRendererTreeProvider : IFormInputRendererTreeProvider
    {
        private readonly IEnumerable<IFormInputRendererBuilder> formInputRendererBuilders;

        public FormInputsRendererTreeProvider(IEnumerable<IFormInputRendererBuilder> formInputRendererBuilders)
        {
            this.formInputRendererBuilders = formInputRendererBuilders ?? throw new ArgumentException(nameof(formInputRendererBuilders));
        }

        public IFormInputRendererBuilder GetFormInputRendererTreeBuilder(FormField formField)
        {
            var builder = formInputRendererBuilders.FirstOrDefault(b => b.IsSupportedDateType(formField.UnderlyneType));
            if (builder is null)
            {
                throw new InvalidOperationException($"Type {formField.Type.FullName} is not supported in edit forms");
            }

            return builder;
        }
    }
}
