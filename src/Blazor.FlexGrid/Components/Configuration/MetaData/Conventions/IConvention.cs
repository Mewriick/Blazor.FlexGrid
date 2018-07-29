using System;

namespace Blazor.FlexGrid.Components.Configuration.MetaData.Conventions
{
    public interface IConvention
    {
        void Apply(Type type);
    }
}
