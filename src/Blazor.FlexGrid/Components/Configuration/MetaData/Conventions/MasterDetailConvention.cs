using Blazor.FlexGrid.Components.Configuration.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            var clrType = internalEntityTypeBuilder.Metadata.ClrType;
            var collectionProperties = GetCollectionProperties(clrType);
            foreach (var property in collectionProperties)
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

        private IEnumerable<PropertyInfo> GetCollectionProperties(Type type)
            => type.GetProperties().Where(t => typeof(IEnumerable).IsAssignableFrom(t.PropertyType));

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
