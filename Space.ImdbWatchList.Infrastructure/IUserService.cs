﻿using System.Threading;
using System.Threading.Tasks;
using Space.ImdbWatchList.Models.ViewModel;

namespace Space.ImdbWatchList.Infrastructure
{
    public interface IUserService
    {
        Task AddFilmToWatchListAsync(int userId, string filmId, CancellationToken ct);
    }
}