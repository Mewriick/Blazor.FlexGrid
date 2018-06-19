using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class Model : Annotatable, IModel
    {
        public EntityType AddEntityType(Type clrType)
        {
            return AddEntityType(clrType);
        }

        public EntityType FindEntityType(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EntityType> GetEntityTypes()
        {
            throw new NotImplementedException();
        }

        IEntityType IModel.AddEntityType(Type clrType)
        {
            var entityType = new EntityType(this);

            return entityType;
        }

        IEntityType IModel.FindEntityType(string name)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IEntityType> IModel.GetEntityTypes()
        {
            throw new NotImplementedException();
        }
    }
}
