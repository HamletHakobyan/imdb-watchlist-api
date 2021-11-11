using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Space.ImdbWatchList.Dto;
using Space.ImdbWatchList.Infrastructure;

namespace Space.ImdbWatchList.Api.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FilmsController : ControllerBase
    {
        private readonly IFilmService _filmService;
        private readonly ILogger<FilmsController> _logger;

        public FilmsController(IFilmService filmService, ILogger<FilmsController> logger)
        {
            _filmService = filmService;
            _logger = logger;
        }

        /// <summary>
        /// Search films on IMDB
        /// </summary>
        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FilmDto>>> SearchFilmsAsync(string name, CancellationToken ct)
        {
            try
            {
                var films = await _filmService.SearchFilmsAsync(name, ct);
                if (films.Any())
                {
                    return films;
                }

                _logger.LogInformation($"Search film by name {name} returns no element.");
                return NotFound(name);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unhandled exception in {nameof(IFilmService.SearchFilmsAsync)}");
                return Problem(detail: e.Message);
            }
        }
    }
}
