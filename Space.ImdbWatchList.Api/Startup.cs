using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Space.ImdbWatchList.ClientServices;
using Space.ImdbWatchList.Common;
using Space.ImdbWatchList.Data;
using Space.ImdbWatchList.Infrastructure;
using Space.ImdbWatchList.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Space.ImdbWatchList.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ImdbSettings>(Configuration.GetSection(ImdbSettings.SectionName));

            services.AddControllers();

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = ApiVersion.Parse("1");
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddSwaggerGen();
            services.AddOptions<SwaggerGenOptions>()
                .Configure<IApiVersionDescriptionProvider>((options, provider) =>
                {
                    foreach (var apiVersionDescription in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(apiVersionDescription.GroupName, new OpenApiInfo
                        {
                            Title = "IMDB watchlist API",
                            Description = "Space IMDB WatchList Server",
                            Version = apiVersionDescription.ApiVersion.ToString(),
                        });

                        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                    }
                });

            services.AddHttpClient<IImdbClientService, ImdbClientService>((provider, client) =>
            {
                var imdbSettings = provider.GetRequiredService<IOptions<ImdbSettings>>().Value;
                client.BaseAddress = new Uri(imdbSettings.Host);
            });

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IFilmService, FilmService>();
            services.AddTransient<IWatchListService, WatchListService>();

            services.AddDbContext<ImdbWatchListDbContext>(
                builder => builder.UseSqlServer(Configuration.GetConnectionString("mssqlConnectionString")));

            services.AddTransient<UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var apiVersionDescription in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint(
                        $"/swagger/{apiVersionDescription.GroupName}/swagger.json",
                        apiVersionDescription.GroupName.ToUpperInvariant()
                    );
                }

            });
        }
    }
}
