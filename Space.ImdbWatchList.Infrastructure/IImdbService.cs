using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Space.ImdbWatchList.Models;
using Space.ImdbWatchList.Models.ViewModel;

namespace Space.ImdbWatchList.Infrastructure
{
    public interface IImdbService
    {
        Task<List<FilmVm>> SearchFilmsAsync(string name, CancellationToken ct);
    }
}
