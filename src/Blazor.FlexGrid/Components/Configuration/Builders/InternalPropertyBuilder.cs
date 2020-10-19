using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components;
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

        public bool HasHeaderStyle(string style)
            => HasAnnotation(GridViewAnnotationNames.ColumnHeaderStyle, style);

        public bool HasCaption(string caption)
            => HasAnnotation(GridViewAnnotationNames.ColumnCaption, caption);

        public bool IsVisible(bool isVisible)
            => HasAnnotation(GridViewAnnotationNames.ColumnIsVisible, isVisible);

        public bool IsSortable(bool isSortable)
            => HasAnnotation(GridViewAnnotationNames.ColumnIsSortable, isSortable);

        public bool IsFilterable(StringComparison textComparison)
        {
            HasAnnotation(GridViewAnnotationNames.ColumnIsFilterable, true);
            HasAnnotation(GridViewAnnotationNames.ColumnFilterTextComparison, textComparison);

            return true;
        }

        public bool HasOrder(int order)
            => HasAnnotation(GridViewAnnotationNames.ColumnOrder, order);

        public bool HasValueFormatter<TProperty>(Expression<Func<TProperty, string>> valueFormatterExpression, string defaultValue = default)
            => HasAnnotation(GridViewAnnotationNames.ColumnValueFormatter, new ValueFormatter<TProperty>(valueFormatterExpression, defaultValue: defaultValue));

        public bool HasCompositeValueFormatter<TInputValue>(Expression<Func<TInputValue, string>> valueFormatterExpression)
            => HasAnnotation(GridViewAnnotationNames.ColumnValueFormatter, new ValueFormatter<TInputValue>(valueFormatterExpression, ValueFormatterType.WholeRowObject));

        public bool HasBlazorComponentValue<TInputValue>(RenderFragment<TInputValue> renderFragment)
        {
            if (Metadata.FindAnnotation(GridViewAnnotationNames.ColumnValueBlazorComponent) is NullAnnotation)
            {
                return HasAnnotation(GridViewAnnotationNames.ColumnValueBlazorComponent, new RenderFragmentAdapter<TInputValue>(renderFragment));
            }

            return false;
        }

        public bool HasBlazorEditComponent<TInputValue>(Func<EditColumnContext, RenderFragment<TInputValue>> renderFragmentBuilder)
        {
            if (Metadata.FindAnnotation(GridViewAnnotationNames.ColumnEditBlazorComponentBuilder) is NullAnnotation)
            {
                Func<EditColumnContext, IRenderFragmentAdapter> builder
                    = context =>
                    {
                        return new RenderFragmentAdapter<TInputValue>(renderFragmentBuilder.Invoke(context));
                    };

                return HasAnnotation(GridViewAnnotationNames.ColumnEditBlazorComponentBuilder, builder);
            }

            return false;
        }

        public bool HasReadPermissionRestriction(Func<ICurrentUserPermission, bool> permissionRestrictionFunc)
            => HasAnnotation(GridViewAnnotationNames.ColumnReadPermission, permissionRestrictionFunc);

        public bool HasWritePermissionRestriction(Func<ICurrentUserPermission, bool> permissionRestrictionFunc)
            => HasAnnotation(GridViewAnnotationNames.ColumnWrtiePermission, permissionRestrictionFunc);
    }
}
