using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Space.ImdbWatchList.Dto;

namespace Space.ImdbWatchList.Infrastructure
{
    public interface IWatchListService
    {
        Task<List<WatchListDto>> GetWatchlistItemsByUserIdAsync(int id, CancellationToken ct = default);
        Task MarkFilmAsWatchedAsync(int id, string filmId, CancellationToken ct = default);
        Task<List<OfferDataDto>> GetEmailOfferDataAsync(CancellationToken ct = default);
        Task SetOfferedDateAsync(List<(int userId, string filmId)> watchListItems);
    }
}