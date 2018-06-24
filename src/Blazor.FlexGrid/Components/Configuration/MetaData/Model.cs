using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class Model : Annotatable, IModel
    {
        private readonly SortedDictionary<string, EntityType> entityTypes;
        private readonly Dictionary<Type, EntityType> entityTypesMap;

        public Model()
        {
            this.entityTypes = new SortedDictionary<string, EntityType>();
            this.entityTypesMap = new Dictionary<Type, EntityType>();
        }


        public EntityType AddEntityType(Type clrType)
        {
            var findedEntityType = FindEntityType(clrType);
            if (findedEntityType != null)
            {
                return findedEntityType;
            }

            var entityType = new EntityType(clrType, this);

            entityTypes.Add(entityType.Name, entityType);
            entityTypesMap.Add(clrType, entityType);

            return entityType;
        }

        public EntityType FindEntityType(Type clrType)
            => entityTypesMap.TryGetValue(clrType, out var entityType)
                ? entityType
                : null;

        public EntityType FindEntityType(string name)
            => entityTypes.TryGetValue(name, out var entityType)
                ? entityType
                : null;

        public IEnumerable<EntityType> GetEntityTypes() => entityTypes.Values;


        IEntityType IModel.AddEntityType(Type clrType) => AddEntityType(clrType);

        IEntityType IModel.FindEntityType(Type clrType) => FindEntityType(clrType);

        IEnumerable<IEntityType> IModel.GetEntityTypes() => GetEntityTypes();


    }
}
