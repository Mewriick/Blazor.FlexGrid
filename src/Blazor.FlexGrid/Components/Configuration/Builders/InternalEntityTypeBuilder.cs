using Blazor.FlexGrid.Components.Configuration.MetaData;
using System;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Configuration.Builders
{
    public class InternalEntityTypeBuilder : InternalMetadataBuilder<EntityType>
    {
        public override InternalModelBuilder ModelBuilder { get; }

        public InternalEntityTypeBuilder(EntityType metadata, InternalModelBuilder modelBuilder)
            : base(metadata)
        {
            ModelBuilder = modelBuilder;
        }

        public InternalPropertyBuilder Property(MemberInfo memberInfo)
        {
            var property = Metadata.AddProperty(memberInfo);

            return new InternalPropertyBuilder(property, new InternalModelBuilder(Metadata.Model));
        }

        public InternalMasterDetailRelationshipBuilder HasDetailRelationship(Type detailType, string masterPropertyName, string foreignPropertyName)
        {
            var masterDetailRelationship = Metadata.AddDetailRelationship(detailType, masterPropertyName, foreignPropertyName);

            return new InternalMasterDetailRelationshipBuilder(masterDetailRelationship, new InternalModelBuilder(Metadata.Model));
        }

        public InternalMasterDetailRelationshipBuilder HasDetailRelationship(Type detailType)
        {
            var masterDetailRelationship = Metadata.AddDetailRelationship(detailType);

            return new InternalMasterDetailRelationshipBuilder(masterDetailRelationship, new InternalModelBuilder(Metadata.Model));
        }

        public bool UseCssClasses(Action<GridCssClasses> gridCssClassesConfig)
        {
            var gridCssClasses = new GridCssClasses();
            gridCssClassesConfig?.Invoke(gridCssClasses);

            return HasAnnotation(GridViewAnnotationNames.CssClasses, gridCssClasses);
        }

        public bool AppendCssClasses(Action<GridCssClasses> gridCssClassesConfig)
        {
            var gridCssClasses = new GridCssClasses();
            gridCssClassesConfig?.Invoke(gridCssClasses);
            gridCssClasses.AppendDefaultCssClasses(new DefaultGridCssClasses());
            gridCssClasses.AppendDefaultFooterCssClasses(new DefaultFooterCssClasses());
            gridCssClasses.AppendDefaultCreateFormCssClasses(new DefaultCreateFormCssClasses());

            return HasAnnotation(GridViewAnnotationNames.CssClasses, gridCssClasses);
        }

        public bool IsMasterTable()
            => HasAnnotation(GridViewAnnotationNames.IsMasterTable, true);

        public bool IsMasterTable(MasterDetailOptions masterDetailOptions)
        {
            IsMasterTable();
            return HasAnnotation(GridViewAnnotationNames.MasterDetailOptions, masterDetailOptions);
        }

        public bool OnlyShowExplicitProperties()
        {
            return HasAnnotation(GridViewAnnotationNames.OnlyShowExplicitProperties, true);
        }

        public bool EnableGrouping(GlobalGroupingOptions groupingOptions)
            => HasAnnotation(GridViewAnnotationNames.GroupingOptions, groupingOptions);

        public bool AllowInlineEdit(InlineEditOptions inlineEditOptions)
            => HasAnnotation(GridViewAnnotationNames.InlineEditOptions, inlineEditOptions);

        public bool AllowCreateItem(CreateItemOptions createItemOptions)
            => HasAnnotation(GridViewAnnotationNames.CreateItemOptions, createItemOptions);

        public bool HasEmptyItemsMessage(string message)
            => HasAnnotation(GridViewAnnotationNames.EmptyItemsMessage, message);
    }
}
