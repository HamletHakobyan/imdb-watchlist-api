using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Space.ImdbWatchList.Common.Exceptions;
using Space.ImdbWatchList.Data.Entities;
using Space.ImdbWatchList.Models.ViewModel;

namespace Space.ImdbWatchList.Data.Repositories
{
    public class WatchListRepository
    {
        private readonly ImdbWatchListDbContext _dbContext;

        public WatchListRepository(ImdbWatchListDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddFilmToWatchListAsync(int userId, string filmId, CancellationToken ct)
        {
            var watchListItem = await _dbContext.WatchLists.FindAsync(new object[] { userId, filmId }, ct);

            if (watchListItem != null)
            {
                throw new AlreadyInWatchListException($"The film '{watchListItem.Film.Title}' is already in the user's (ID={userId}) watchlist.");
            }

            var watchList = new WatchList
            {
                UserId = userId,
                FilmId = filmId,
            };

            await _dbContext.WatchLists.AddAsync(watchList, ct);
        }

        public async Task<IEnumerable<WatchListVm>> GetWatchListItemsByUserIdAsync(int userId, CancellationToken ct)
        {
            var watchLists = await _dbContext.WatchLists
                .Include(wl => wl.Film)
                .Where(wl => wl.UserId == userId)
                .ToListAsync(ct);

            return watchLists.Select(wl => new WatchListVm
            {
                Added = wl.Added,
                Film = FilmToVm(wl.Film),
                WatchedDate = wl.WatchedDate,
                OfferedDate = wl.OfferedDate,
            });
        }

        public async Task MarkFilmAsWatchedById(int userId, string filmId, CancellationToken ct)
        {
            var watchList = await _dbContext.WatchLists.Include(wl => wl.Film)
                .FirstOrDefaultAsync(wl => wl.UserId == userId && wl.FilmId == filmId, ct);
            if (watchList == null)
            {
                throw new NoSuchItemException($"There is no film with ID {filmId} in the User's (ID={userId}) watchlist.");
            }


            if (watchList.WatchedDate != null)
            {
                throw new AlreadyWatchedException(
                    $"The film '{watchList.Film.Title}' already is marked as watched in user's (ID={userId}) watchlist.");
            }

            watchList.WatchedDate = DateTime.UtcNow;
        }

        // this will be removed after adding AutoMapper
        private static FilmVm FilmToVm(Film film)
        {
            return new FilmVm
            {
                Id = film.Id,
                Title = film.Title,
            };
        }
    }
}