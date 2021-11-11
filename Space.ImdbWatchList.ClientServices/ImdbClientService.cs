using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Space.ImdbWatchList.ClientServices.Extensions;
using Space.ImdbWatchList.Common;
using Space.ImdbWatchList.Infrastructure;
using Space.ImdbWatchList.Models.ResponseModel;
using Space.ImdbWatchList.Models.ViewModel;

namespace Space.ImdbWatchList.ClientServices
{
    public class ImdbClientService : IImdbClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ImdbSettings _imdbSettings;

        public ImdbClientService(HttpClient httpClient, IOptions<ImdbSettings> imdbSettingsOptions)
        {
            _httpClient = httpClient;
            _imdbSettings = imdbSettingsOptions.Value;
        }

        public async Task<List<FilmVm>> SearchFilmsAsync(string name, CancellationToken ct = default)
        {
            var searchResponseVm = await _httpClient.GetFromJsonAsync<SearchResponseVm>(
                $"/api/search/{_imdbSettings.ApiKey}/{name}", cancellationToken:ct);


            if (!string.IsNullOrWhiteSpace(searchResponseVm?.ErrorMessage))
            {
                throw new Exception(searchResponseVm.ErrorMessage);
            }

            return searchResponseVm?.Results.Select(r => new FilmVm
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
            }).ToList();
        }

        public async Task<RatingVm> GetFilmRatingById(string id, CancellationToken ct = default)
        {
            var ratingResponseVm = await _httpClient.GetFromJsonAsync<RatingsResponseVm>(
                $"/api/ratings/{_imdbSettings.ApiKey}/{id}", cancellationToken:ct);

            if (!string.IsNullOrWhiteSpace(ratingResponseVm.ErrorMessage))
            {
                throw new Exception(ratingResponseVm.ErrorMessage);
            }

            return new RatingVm
            {
                FilmId = ratingResponseVm.IMDbId,
                ImdbRating = decimal.Parse(ratingResponseVm.IMDb, CultureInfo.InvariantCulture),
            };
        }

        public async Task<FilmFullVm> GetFullTitleById(string id, CancellationToken ct)
        {
            // for this particular request {lang?} is not optional and return uninformative error when omitted
            var fullTitleResp = await _httpClient.GetFromJsonAsync<FullTitleResponseVm>(
                $"/en/api/title/{_imdbSettings.ApiKey}/{id}/Posters,Ratings,Wikipedia", cancellationToken: ct);

            if (!string.IsNullOrWhiteSpace(fullTitleResp!.ErrorMessage))
            {
                throw new Exception(fullTitleResp.ErrorMessage);
            }

            return new FilmFullVm
            {
                Id = fullTitleResp.Id,
                Title = fullTitleResp.Title,
                Rating = decimal.Parse(fullTitleResp.Ratings.IMDb, CultureInfo.InvariantCulture),
                Posters = fullTitleResp.Posters,
                WikipediaInfo = fullTitleResp.Wikipedia,
            };
        }
    }
}
