using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Configuration.MetaData.Conventions
{
    public class ConventionsSet
    {
        private readonly HashSet<Type> conventionsRunnedTypes;

        private readonly IGridConfigurationProvider gridConfigurationProvider;

        public virtual IList<IConvention> Conventions { get; }

        public ConventionsSet(IGridConfigurationProvider gridConfigurationProvider)
        {
            this.gridConfigurationProvider = gridConfigurationProvider ?? throw new ArgumentNullException(nameof(gridConfigurationProvider));
            this.conventionsRunnedTypes = new HashSet<Type>();

            Conventions = new List<IConvention>()
            {
                new MasterDetailConvention(gridConfigurationProvider)
            };
        }

        public virtual void ApplyConventions(Type type)
        {
            if (conventionsRunnedTypes.Contains(type))
            {
                return;
            }

            foreach (var convention in Conventions)
            {
                convention.Apply(type);
            }

            conventionsRunnedTypes.Add(type);
        }
    }
}
