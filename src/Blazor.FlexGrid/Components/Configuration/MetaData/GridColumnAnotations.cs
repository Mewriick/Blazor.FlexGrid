using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Components.Renderers;
using System;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class GridColumnAnotations : IGridViewColumnAnotations
    {
        public const int DefaultOrder = 5000;

        private readonly IProperty propertyMetadata;

        public string Caption
        {
            get
            {
                var captionAnnotationValue = Annotations[GridViewAnnotationNames.ColumnCaption];
                if (captionAnnotationValue is NullAnotationValue)
                {
                    return propertyMetadata.Name;
                }

                return captionAnnotationValue.ToString();
            }
        }

        public int Order
        {
            get
            {
                var orderAnnotationValue = Annotations[GridViewAnnotationNames.ColumnOrder];
                if (orderAnnotationValue is NullAnotationValue)
                {
                    return DefaultOrder;
                }

                return (int)orderAnnotationValue;
            }
        }

        public bool IsVisible
        {
            get
            {
                var orderAnnotationValue = Annotations[GridViewAnnotationNames.ColumnIsVisible];
                if (orderAnnotationValue is NullAnotationValue)
                {
                    return true;
                }

                return (bool)orderAnnotationValue;
            }
        }

        public bool IsSortable
        {
            get
            {
                var sortAnnotationValue = Annotations[GridViewAnnotationNames.ColumnIsSortable];
                if (sortAnnotationValue is NullAnotationValue)
                {
                    return false;
                }

                return (bool)sortAnnotationValue;
            }
        }

        public ValueFormatter ValueFormatter
        {
            get
            {
                var formatterValueAnnotation = Annotations[GridViewAnnotationNames.ColumnValueFormatter];
                if (formatterValueAnnotation is NullAnotationValue)
                {
                    return new DefaultValueFormatter();
                }

                return formatterValueAnnotation as ValueFormatter;
            }
        }

        public RenderFragmentAdapter SpecialColumnValue
        {
            get
            {
                var specialColumnValue = Annotations[GridViewAnnotationNames.ColumnValueBlazorComponent];
                if (specialColumnValue is NullAnotationValue)
                {
                    return null;
                }

                return specialColumnValue as RenderFragmentAdapter;
            }
        }

        protected IAnnotatable Annotations { get; }

        public GridColumnAnotations(IProperty property)
        {
            Annotations = property ?? throw new ArgumentNullException(nameof(property));
            this.propertyMetadata = property;
        }
    }
}
