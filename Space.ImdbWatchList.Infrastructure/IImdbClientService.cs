using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Space.ImdbWatchList.Models;
using Space.ImdbWatchList.Models.ResponseModel;
using Space.ImdbWatchList.Models.ViewModel;

namespace Space.ImdbWatchList.Infrastructure
{
    public interface IImdbClientService
    {
        Task<List<FilmVm>> SearchFilmsAsync(string name, CancellationToken ct);

        Task<RatingVm> GetFilmRatingById(string id, CancellationToken ct);
        Task<FilmFullVm> GetFullTitleById(string id, CancellationToken ct);
    }
}
