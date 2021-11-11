using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Space.ImdbWatchList.Dto;

namespace Space.ImdbWatchList.Infrastructure
{
    public interface IWatchListService
    {
        Task<List<WatchListDto>> GetWatchlistItemsByUserIdAsync(int id, CancellationToken ct);
        Task MarkFilmAsWatchedAsync(int id, string filmId, CancellationToken ct);
    }
}