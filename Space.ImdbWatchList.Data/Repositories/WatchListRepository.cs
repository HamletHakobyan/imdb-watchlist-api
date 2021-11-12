using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            var watchListItem = await _dbContext.WatchLists.FindAsync(new object[] { userId, filmId }, ct).ConfigureAwait(false);

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
                .Include(wl => wl.User)
                .Where(wl => wl.UserId == userId)
                .ToListAsync(ct)
                .ConfigureAwait(false);

            return watchLists.Select(wl => new WatchListVm
            {
                Added = wl.Added,
                Film = FilmToVm(wl.Film),
                User = UserToVm(wl.User),
                WatchedDate = wl.WatchedDate,
                OfferedDate = wl.OfferedDate,
            });
        }

        public async Task MarkFilmAsWatchedById(int userId, string filmId, CancellationToken ct)
        {
            var watchList = await _dbContext.WatchLists
                .Include(wl => wl.Film)
                .FirstOrDefaultAsync(wl => wl.UserId == userId && wl.FilmId == filmId, ct)
                .ConfigureAwait(false);
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

        public async Task<List<OfferDataVm>> GetUnwatchedGroupedByUserAsync(CancellationToken ct)
        {
            try
            {
                // find users which have more than 3 unwatched films
                var usersToBeOffered = await _dbContext.WatchLists
                    .Where(wl => wl.WatchedDate == null)
                    .GroupBy(wl => wl.UserId)
                    .Where(g => g.Count() > 3)
                    .Select(g => g.Key)
                    .ToListAsync(ct);


                var offerDataList = await _dbContext.Users
                    .Where(u => usersToBeOffered.Contains(u.Id))
                    .Include(u => u.FilmsInWatchList).ThenInclude(f => f.Film.Posters)
                    .SelectMany(u => u.FilmsInWatchList
                        .Where(wl => wl.WatchedDate == null
                                     && (wl.OfferedDate == null ||
                                         EF.Functions.DateDiffMonth(wl.OfferedDate, DateTime.UtcNow) > 1))
                        .OrderByDescending(wl => wl.Film.Rating).Take(1)
                    )
                    .Select(d => new { d.User, d.Film })
                    .ToListAsync(ct);

                return offerDataList.Select(od =>
                {
                    var film = od.Film;
                    return new OfferDataVm
                    {
                        User = UserToVm(od.User),
                        Film = new OfferFilmVm
                        {
                            Id = film.Id,
                            Rating = film.Rating,
                            Title = film.Title,
                            WikiShortDesc = film.WikiDescription,
                            Posters = film.Posters.Select(p => new PosterVm
                            {
                                Id = p.Id,
                                Link = p.Link,
                                Height = p.Height,
                                Width = p.Width,
                            }).ToList(),
                        }
                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task SetItemOfferedInWatchListAsync(List<(int userId, string filmId)> watchListItem)
        {
            foreach (var (userId, filmId) in watchListItem)
            {
                var item = await _dbContext.WatchLists.FindAsync(userId, filmId);
                item.OfferedDate = DateTime.UtcNow;
            }
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

        private static PosterVm PosterToVm(Poster poster)
        {
            return new PosterVm
            {
                Id = poster.Id,
                Link = poster.Link,
                Height = poster.Height,
                Width = poster.Width,
            };
        }

        private static UserVm UserToVm(User user)
        {
            return new UserVm
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
            };
        }
    }
}