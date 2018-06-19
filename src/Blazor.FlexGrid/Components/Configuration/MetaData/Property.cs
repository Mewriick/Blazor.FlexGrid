using System;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class Property : Annotatable, IProperty
    {
        public string Name { get; }

        public Type ClrType { get; }

        public EntityType DeclaringType { get; }


        public Property(string name, Type clrType, EntityType declaringType)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            ClrType = clrType ?? throw new ArgumentNullException(nameof(clrType));
            DeclaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
        }
    }
}
