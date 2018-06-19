using System;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class GridColumnAnnotations : IGridViewColumnAnnotations
    {
        public string Caption => throw new NotImplementedException();

        public int Order => throw new NotImplementedException();

        public bool IsVisible => throw new NotImplementedException();

        protected IAnnotatable Annotations { get; }

        public GridColumnAnnotations(IProperty property)
        {
            Annotations = property ?? throw new ArgumentNullException(nameof(property));
        }
    }
}
