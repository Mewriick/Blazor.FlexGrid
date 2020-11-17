namespace Blazor.FlexGrid.State
{
    internal interface IStateCache
    {
        int Count { get; }

        void SetStateValue<T>(string key, T value);

        bool TryGetStateValue<T>(string key, out T value);

        void RemoveStateValue(string key);
    }
}
