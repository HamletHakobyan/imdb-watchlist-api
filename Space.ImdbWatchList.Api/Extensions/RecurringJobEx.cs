using System;
using System.Collections.Generic;
using System.Linq;
using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Space.ImdbWatchList.Infrastructure;
using Space.ImdbWatchList.RecurringJobs;

namespace Space.ImdbWatchList.Api.Extensions
{
    public static class RecurringJobEx
    {
        public static IServiceCollection RegisterRecurringJobImplementations(this IServiceCollection services)
        {
            var jobs = typeof(OfferSender).Assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(IRecurringJob)) == typeof(IRecurringJob));

            foreach (var job in jobs)
            {
                services.Add(new ServiceDescriptor(typeof(IRecurringJob), job, ServiceLifetime.Transient));
            }

            return services;
        }

        public static void RegisterAndConfigureRecurringJobs(
            this IConfiguration configuration,
            IEnumerable<IRecurringJob> recurringJobs)
        {
            var settings = new HangfireSettings();
            configuration.GetSection(HangfireSettings.SectionName).Bind(settings);

            foreach (var recurringJob in recurringJobs)
            {
                if (!settings.CronExpressions.TryGetValue(recurringJob.GetType().Name, out var cronExpression))
                {
                    continue;
                }

                RecurringJob.AddOrUpdate(() => recurringJob.Execute(), cronExpression);
            }
        }

        private class HangfireSettings
        {
            public const string SectionName = "Hangfire";
            public Dictionary<string, string> CronExpressions { get; set; }
        }
    }
}