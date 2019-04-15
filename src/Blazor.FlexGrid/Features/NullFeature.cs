namespace Blazor.FlexGrid.Features
{
    public class NullFeature : IFeature
    {
        public static NullFeature Instance = new NullFeature();

        public string Name => nameof(NullFeature);
    }
}
