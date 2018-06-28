using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Configuration.Builders
{
    public class InternalPropertyBuilder : InternalMetadataBuilder<Property>
    {
        public override InternalModelBuilder ModelBuilder { get; }

        public InternalPropertyBuilder(Property metadata, InternalModelBuilder internalModelBuilder)
            : base(metadata)
        {
            ModelBuilder = internalModelBuilder ?? throw new ArgumentNullException(nameof(internalModelBuilder));
        }

        public bool HasCaption(string caption)
            => HasAnnotation(GridViewAnnotationNames.ColumnCaption, caption);

        public bool IsVisible(bool isVisible)
            => HasAnnotation(GridViewAnnotationNames.ColumnIsVisible, isVisible);

        public bool IsSortable(bool isSortable)
            => HasAnnotation(GridViewAnnotationNames.ColumnIsSortable, isSortable);

        public bool HasOrder(int order)
            => HasAnnotation(GridViewAnnotationNames.ColumnOrder, order);

        public bool HasValueFormatter<TProperty>(Expression<Func<TProperty, string>> valueFormatterExpression)
            => HasAnnotation(GridViewAnnotationNames.ColumnValueFormatter, new ValueFormatter<TProperty>(valueFormatterExpression));
    }
}
