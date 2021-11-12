using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Space.ImdbWatchList.Common.Exceptions;
using Space.ImdbWatchList.Data;
using Space.ImdbWatchList.Infrastructure;
using Space.ImdbWatchList.Models.ViewModel;

namespace Space.ImdbWatchList.Services
{
    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IImdbClientService _imdbClientService;
        private readonly ILogger<UserService> _logger;

        public UserService(UnitOfWork unitOfWork, IImdbClientService imdbClientService, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _imdbClientService = imdbClientService;
            _logger = logger;
        }
        public async Task AddFilmToWatchListAsync(int userId, string filmId, CancellationToken ct)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(userId, ct).ConfigureAwait(false);
            if (user == null)
            {
                throw new UserNotFoundException($"User with given Id = {userId} doesn't exists.");
            }

            try
            {
                var filmVm = await _unitOfWork.Films.GetByIdAsync(filmId, ct).ConfigureAwait(false);
                string newFilmId;

                if (filmVm == null)
                {
                    var fullTitle = await _imdbClientService.GetFullTitleById(filmId, ct).ConfigureAwait(false);
                    newFilmId = await _unitOfWork.Films.AddFilmAsync(fullTitle, ct).ConfigureAwait(false);
                }
                else
                {
                    newFilmId = filmVm.Id;
                }

                await _unitOfWork.WatchLists.AddFilmToWatchListAsync(userId, newFilmId, ct).ConfigureAwait(false);
                
                await _unitOfWork.CompleteAsync(ct).ConfigureAwait(false);
            }
            catch(DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex.Message);
                // need more consistent handling of concurrency issues
                throw;
            }



        }

        public async Task<List<UserVm>> GetUsersAsync(CancellationToken ct)
        {
            return await _unitOfWork.Users.GetAllAsync(ct).ConfigureAwait(false);
        }
    }
}