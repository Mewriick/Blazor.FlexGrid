using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using System;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class GridColumnAnnotations : IGridViewColumnAnnotations
    {
        private readonly IProperty propertyMetadata;

        public string Caption
        {
            get
            {
                var captionAnnotation = Annotations[GridViewAnnotationNames.ColumnCaption];
                if (captionAnnotation is NullAnotationValue)
                {
                    return propertyMetadata.Name;
                }

                return captionAnnotation.ToString();
            }
        }

        public int Order
        {
            get
            {
                var orderAnnotation = Annotations[GridViewAnnotationNames.ColumnOrder];
                if (orderAnnotation is NullAnotationValue)
                {
                    return 0;
                }

                return (int)orderAnnotation;
            }
        }

        public bool IsVisible
        {
            get
            {
                var orderAnnotation = Annotations[GridViewAnnotationNames.ColumnIsVisible];
                if (orderAnnotation is NullAnotationValue)
                {
                    return true;
                }

                return (bool)orderAnnotation;
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


        protected IAnnotatable Annotations { get; }


        public GridColumnAnnotations(IProperty property)
        {
            Annotations = property ?? throw new ArgumentNullException(nameof(property));
            this.propertyMetadata = property;
        }
    }
}
