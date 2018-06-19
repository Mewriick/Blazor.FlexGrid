using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{

    /// <summary>
    /// Contract for exposing Annotations
    /// </summary>
    public interface IAnnotatable
    {
        object this[string name] { get; }

        IAnnotation FindAnnotation(string name);

        IEnumerable<IAnnotation> GetAllAnnotations();
    }
}
