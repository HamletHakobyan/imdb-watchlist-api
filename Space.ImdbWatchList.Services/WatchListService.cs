using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Space.ImdbWatchList.Data;
using Space.ImdbWatchList.Dto;
using Space.ImdbWatchList.Infrastructure;
using Space.ImdbWatchList.Models.ViewModel;

namespace Space.ImdbWatchList.Services
{
    public class WatchListService : IWatchListService
    {
        private readonly UnitOfWork _unitOfWork;

        public WatchListService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<WatchListDto>> GetWatchlistItemsByUserIdAsync(int id, CancellationToken ct)
        {
            var watchListVms =
                await _unitOfWork.WatchLists.GetWatchListItemsByUserIdAsync(id, ct).ConfigureAwait(false);

            return watchListVms.Select(WatchListVmToDto).ToList();


            WatchListDto WatchListVmToDto(WatchListVm watchListVm)
            {
                return new WatchListDto
                {
                    Film = FilmVmToDto(watchListVm.Film),
                    Added = watchListVm.Added,
                    WatchedDate = watchListVm.WatchedDate,
                    OfferedDate = watchListVm.OfferedDate,
                };
            }

            FilmDto FilmVmToDto(FilmVm filmVm)
            {
                return new FilmDto
                {
                    Id = filmVm.Id,
                    Title = filmVm.Title,
                };
            }

        }

        public async Task MarkFilmAsWatchedAsync(int id, string filmId, CancellationToken ct)
        {
            await _unitOfWork.WatchLists.MarkFilmAsWatchedById(id, filmId, ct).ConfigureAwait(false);
            await _unitOfWork.CompleteAsync(ct).ConfigureAwait(false);
        }

        public async Task<List<OfferDataDto>> GetEmailOfferDataAsync(CancellationToken ct)
        {
            var offerDataVms = await _unitOfWork.WatchLists.GetUnwatchedGroupedByUserAsync(ct).ConfigureAwait(false);
            return offerDataVms.Select(od =>

                new OfferDataDto
                {
                    FilmId = od.Film.Id,
                    UserId = od.User.Id,
                    UserName = od.User.Name,
                    Email = od.User.Email,
                    Rating = od.Film.Rating,
                    Title = od.Film.Title,
                    Poster = od.Film.Posters.OrderByDescending(p => p.Height * p.Width).Select(p => new PosterDto
                    {
                        Id = p.Id,
                        Link = p.Link,
                    }).FirstOrDefault(),
                    WikiShortDesc = od.Film.WikiShortDesc,
                }).ToList();
        }

        public async Task SetOfferedDateAsync(List<(int userId, string filmId)> watchListItems)
        {
            await _unitOfWork.WatchLists.SetItemOfferedInWatchListAsync(watchListItems);
            await _unitOfWork.CompleteAsync();
        }
    }
}