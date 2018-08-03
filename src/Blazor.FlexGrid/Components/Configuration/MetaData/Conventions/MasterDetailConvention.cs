using Blazor.FlexGrid.Components.Configuration.Builders;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Configuration.MetaData.Conventions
{
    public class MasterDetailConvention : BaseConvention
    {
        private const int DefaultPageSize = 10;

        public MasterDetailConvention(IGridConfigurationProvider gridConfigurationProvider)
            : base(gridConfigurationProvider)
        {
        }

        public override void Apply(InternalEntityTypeBuilder internalEntityTypeBuilder)
        {
            var entityType = internalEntityTypeBuilder.Metadata;
            foreach (var property in entityType.ClrTypeCollectionProperties)
            {
                var propertyType = property.PropertyType;
                var masterDetailConfiguration = internalEntityTypeBuilder.Metadata.FindDetailRelationship(propertyType);
                if (masterDetailConfiguration is null)
                {
                    var relationShipBuilder = internalEntityTypeBuilder.HasDetailRelationship(propertyType);
                    relationShipBuilder.HasPageSize(DefaultPageSize);
                    relationShipBuilder.HasCaption(property.Name);
                }
                else
                {
                    ConfigureMasterDetailRelationShip(masterDetailConfiguration, property);
                }
            }
        }

        private void ConfigureMasterDetailRelationShip(IMasterDetailRelationship masterDetailRelationship, PropertyInfo propertyInfo)
        {
            var relationShipBuilder = new InternalMasterDetailRelationshipBuilder(masterDetailRelationship as MasterDetailRelationship, internalModelBuilder);
            var pageSizeAnnotationValue = masterDetailRelationship[GridViewAnnotationNames.DetailTabPageSize];
            var pageCaptionAnnotationValue = masterDetailRelationship[GridViewAnnotationNames.DetailTabPageCaption];

            if (pageSizeAnnotationValue is NullAnotationValue)
            {
                relationShipBuilder.HasPageSize(DefaultPageSize);
            }

            if (pageCaptionAnnotationValue is NullAnotationValue)
            {
                relationShipBuilder.HasCaption(propertyInfo.Name);
            }
        }
    }
}
