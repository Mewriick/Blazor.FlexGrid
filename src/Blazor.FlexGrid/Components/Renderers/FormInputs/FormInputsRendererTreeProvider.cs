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

        public IFormInputRendererBuilder GetFormInputRendererTreeBuilder(Type filedType)
        {
            var builder = formInputRendererBuilders.FirstOrDefault(b => b.IsSupportedDateType(filedType));
            if (builder is null)
            {
                throw new InvalidOperationException($"Type {filedType.FullName} is not supported in edit forms");
            }

            return builder;
        }
    }
}
