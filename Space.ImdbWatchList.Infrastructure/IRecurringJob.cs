using System.Threading.Tasks;

namespace Space.ImdbWatchList.Infrastructure
{
    public interface IRecurringJob
    {
        Task Execute();
    }
}