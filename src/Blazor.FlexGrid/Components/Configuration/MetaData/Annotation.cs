using System;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class Annotation : IAnnotation
    {
        public string Name { get; }

        public object Value { get; }

        public Annotation(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }


    public class NullAnnotation : Annotation
    {
        public static NullAnnotation Instance = new NullAnnotation();

        public NullAnnotation()
            : this("Null", new NullAnotationValue())
        {

        }

        private NullAnnotation(string name, object value)
            : base(name, value)
        {
        }
    }

    public class NullAnotationValue
    {
    }
}
