using System.Threading.Tasks;

namespace Blazor.FlexGrid.Triggers
{
    public interface ITrigger
    {
        Task Execute();
    }
}
