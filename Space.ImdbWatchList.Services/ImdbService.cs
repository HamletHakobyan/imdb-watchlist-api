using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Space.ImdbWatchList.Infrastructure;
using Space.ImdbWatchList.Models;
using Space.ImdbWatchList.Models.Settings;
using Space.ImdbWatchList.Models.ViewModel;

namespace Space.ImdbWatchList.Services
{
    public class ImdbService : IImdbService
    {
        private readonly HttpClient _httpClient;
        private readonly ImdbSettings _imdbSettings;

        public ImdbService(HttpClient httpClient, IOptions<ImdbSettings> imdbSettingsOptions)
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
    }
}
