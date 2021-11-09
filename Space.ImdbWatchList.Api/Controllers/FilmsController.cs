using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Space.ImdbWatchList.Infrastructure;
using Space.ImdbWatchList.Models;
using Space.ImdbWatchList.Models.ViewModel;

namespace Space.ImdbWatchList.Api.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FilmsController : ControllerBase
    {
        private readonly IImdbService _imdbService;

        public FilmsController(IImdbService imdbService)
        {
            _imdbService = imdbService;
        }

        /// <summary>
        /// Search films on IMDB
        /// </summary>
        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<FilmVm>>> SearchFilmsAsync(string name, CancellationToken ct)
        {
            try
            {
                var films = await _imdbService.SearchFilmsAsync(name, ct);
                if (!films.Any())
                {
                    return NotFound(name);
                }

                return films;
            }
            catch (Exception e)
            {
                return Problem(detail: e.Message);
            }
        }
    }
}
