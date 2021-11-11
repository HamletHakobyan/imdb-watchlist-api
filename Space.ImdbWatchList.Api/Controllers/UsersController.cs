using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Space.ImdbWatchList.Common.Exceptions;
using Space.ImdbWatchList.Dto;
using Space.ImdbWatchList.Infrastructure;

namespace Space.ImdbWatchList.Api.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IWatchListService _watchListService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, IWatchListService watchListService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _watchListService = watchListService;
            _logger = logger;
        }

        /// <summary>
        /// Get watchlist items for particular user
        /// </summary>
        [HttpGet]
        [Route("{id:int}/watchlist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<WatchListDto>>> GetWatchListAsync(int id, CancellationToken ct)
        {
            try
            {
                return await _watchListService.GetWatchlistItemsByUserIdAsync(id, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception was been thrown in {nameof(UsersController.GetWatchListAsync)}");
                return Problem(detail: ex.Message);
            }

        }

        /// <summary>
        /// Puts film to the user's watchlist store
        /// </summary>
        [HttpPut]
        [Route("{id:int}/watchlist/{filmId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddFilmToWatchlistAsync(int id,string filmId, CancellationToken ct)
        {
            try
            {
                await _userService.AddFilmToWatchListAsync(id, filmId, ct);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, $"Exception was been thrown in {nameof(UsersController.AddFilmToWatchlistAsync)}");
                return NotFound(ex.Message);
            }
            catch (AlreadyInWatchListException ex)
            {
                _logger.LogError(ex, $"Exception was been thrown in {nameof(UsersController.AddFilmToWatchlistAsync)}");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception was been thrown in {nameof(UsersController.AddFilmToWatchlistAsync)}");
                return Problem(detail: ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Sets particular film as watched in the given user's watchlist store
        /// </summary>
        [HttpPost]
        [Route("{id:int}/watchlist/markaswatched/{filmId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> MarkAsWatchedAsync(int id, string filmId, CancellationToken ct)
        {
            try
            {
                await _watchListService.MarkFilmAsWatchedAsync(id, filmId, ct);
            }
            catch (NoSuchItemException ex)
            {
                _logger.LogError(ex, $"Exception was been thrown in {nameof(UsersController.MarkAsWatchedAsync)}");
                return NotFound(ex.Message);
            }
            catch (AlreadyWatchedException ex)
            {
                _logger.LogError(ex, $"Exception was been thrown in {nameof(UsersController.MarkAsWatchedAsync)}");
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }
}