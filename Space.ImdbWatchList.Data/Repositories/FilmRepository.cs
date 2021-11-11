using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Space.ImdbWatchList.Data.Entities;
using Space.ImdbWatchList.Models.ViewModel;

namespace Space.ImdbWatchList.Data.Repositories
{
    public class FilmRepository
    {
        private readonly ImdbWatchListDbContext _dbContext;

        public FilmRepository(ImdbWatchListDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> AddFilmAsync(FilmFullVm filmFullVm, CancellationToken ct)
        {
            var film = await _dbContext.Films.FindAsync(new [] {filmFullVm.Id}, ct);
            if (film != null)
            {
                return film.Id;
            }

            film = new Film
            {
                Id = filmFullVm.Id,
                Title = filmFullVm.Title,
                Rating = filmFullVm.Rating,
                Posters = FilmFullVmToPoster(filmFullVm),
                WikiDescription = filmFullVm.WikipediaInfo.PlotShort.PlainText,
            };

            var filmEntry = await _dbContext.AddAsync(film, ct);

            return filmEntry.Entity.Id;


            List<Poster> FilmFullVmToPoster(FilmFullVm filmFull)
            {
                return filmFull.Posters.Posters.Select(p => new Poster
                {
                    Id = p.Id,
                    FilmId = filmFullVm.Id,
                    Link = p.Link,
                    Height = p.Height,
                    Width = p.Width,
                }).ToList();
            }
        }

        public async Task<FilmVm> GetByIdAsync(string filmId, CancellationToken ct)
        {
            var film = await _dbContext.Films.FindAsync(new[] { filmId }, ct);
            return new FilmVm
            {
                Id = film.Id,
                Title = film.Title,
            };
        }
    }
}