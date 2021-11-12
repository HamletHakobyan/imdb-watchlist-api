using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Space.ImdbWatchList.Infrastructure;

namespace Space.ImdbWatchList.RecurringJobs
{
    public class OfferSender : IRecurringJob
    {
        private readonly IWatchListService _watchListService;
        private readonly IEmailService _emailService;
        private readonly ILogger<OfferSender> _logger;

        public OfferSender(IWatchListService watchListService, IEmailService emailService, ILogger<OfferSender> logger)
        {
            _watchListService = watchListService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Execute()
        {
            using var semaphore = new SemaphoreSlim(8, 8);
            try
            {
                var offerDataDtos = await _watchListService.GetEmailOfferDataAsync().ConfigureAwait(false);
                var tasks = offerDataDtos
                    .Select(async emailOfferDto =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            await Task.Run(() => _emailService.SendEmail(emailOfferDto));
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }).ToList();
                await Task.WhenAll(tasks);

                var watchListItems = offerDataDtos.Select(od => (od.UserId, od.FilmId)).ToList();
                await _watchListService.SetOfferedDateAsync(watchListItems);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception was been thrown in {nameof(OfferSender)}.{nameof(Execute)}");
            }
        }


    }
}
