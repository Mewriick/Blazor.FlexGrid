using Microsoft.AspNetCore.Components;
using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    public interface IRenderFragmentAdapter
    {
        RenderFragment GetColumnFragment(object item);
    }

    public interface IRenderFragmentAdapter<in TItem> : IRenderFragmentAdapter
    {
        RenderFragment GetColumnFragment(TItem item);
    }

    public class RenderFragmentAdapter<TItem> : IRenderFragmentAdapter<TItem>
    {
        private readonly RenderFragment<TItem> renderFragment;

        public RenderFragmentAdapter(RenderFragment<TItem> renderFragment)
        {
            this.renderFragment = renderFragment ?? throw new ArgumentNullException(nameof(renderFragment));
        }

        public RenderFragment GetColumnFragment(TItem item)
            => renderFragment(item);

        public RenderFragment GetColumnFragment(object item)
            => GetColumnFragment((TItem)item);
    }
}
