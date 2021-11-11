using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Space.ImdbWatchList.Dto;

namespace Space.ImdbWatchList.Infrastructure
{
    public interface IFilmService
    {
        Task<List<FilmDto>> SearchFilmsAsync(string name, CancellationToken ct);
    }
}