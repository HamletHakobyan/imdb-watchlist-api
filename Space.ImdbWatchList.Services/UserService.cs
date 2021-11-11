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
        public async Task AddFilmToWatchListAsync(int userId, string filmId, CancellationToken ct = default)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(userId, ct);
            if (user == null)
            {
                throw new UserNotFoundException($"User with given Id = {userId} doesn't exists.");
            }

            try
            {
                var filmVm = await _unitOfWork.Films.GetByIdAsync(filmId, ct);
                string newFilmId;

                if (filmVm == null)
                {
                    var fullTitle = await _imdbClientService.GetFullTitleById(filmId, ct);
                    newFilmId = await _unitOfWork.Films.AddFilmAsync(fullTitle, ct);
                }
                else
                {
                    newFilmId = filmVm.Id;
                }

                await _unitOfWork.WatchLists.AddFilmToWatchListAsync(userId, newFilmId, ct);
                
                await _unitOfWork.CompleteAsync(ct);
            }
            catch(DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex.Message);
                // need more consistent handling of concurrency issues
                throw;
            }



        }
    }
}