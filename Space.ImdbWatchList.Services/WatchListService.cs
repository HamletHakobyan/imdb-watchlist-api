using System;
using System.Collections.Generic;
using System.Linq;
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
            var watchListVms = await _unitOfWork.WatchLists.GetWatchListItemsByUserIdAsync(id, ct);

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
            await _unitOfWork.WatchLists.MarkFilmAsWatchedById(id, filmId, ct);
            await _unitOfWork.CompleteAsync(ct);
        }
    }
}