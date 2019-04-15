namespace Blazor.FlexGrid.Features
{
    public interface IFeatureCollection
    {
        TFeature Get<TFeature>() where TFeature : IFeature;

        void Set<TFeature>(TFeature instance) where TFeature : IFeature;

        bool Contains<TFeature>() where TFeature : IFeature;
    }
}
