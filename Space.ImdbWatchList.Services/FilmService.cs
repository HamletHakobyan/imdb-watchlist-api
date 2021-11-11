using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Space.ImdbWatchList.Dto;
using Space.ImdbWatchList.Infrastructure;

namespace Space.ImdbWatchList.Services
{
    public class FilmService : IFilmService
    {
        private readonly IImdbClientService _imdbClientService;
        private readonly ILogger<FilmService> _logger;

        public FilmService(IImdbClientService imdbClientService, ILogger<FilmService> logger)
        {
            _imdbClientService = imdbClientService;
            _logger = logger;
        }
        public async Task<List<FilmDto>> SearchFilmsAsync(string name, CancellationToken ct = default)
        {
            var filmVms = await _imdbClientService.SearchFilmsAsync(name, ct);
            return filmVms.Select(fm => new FilmDto
            {
                Id = fm.Id,
                Title = fm.Title,
            }).ToList();
        }
    }
}