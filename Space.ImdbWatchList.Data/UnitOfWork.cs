using System;
using System.Threading;
using System.Threading.Tasks;
using Space.ImdbWatchList.Data.Repositories;

namespace Space.ImdbWatchList.Data
{
    public class UnitOfWork : IAsyncDisposable, IDisposable
    {
        private readonly ImdbWatchListDbContext _context;

        public UnitOfWork(ImdbWatchListDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Films = new FilmRepository(_context);
            WatchLists = new WatchListRepository(_context);
        }

        public UserRepository Users { get; }
        public FilmRepository Films { get; }
        public WatchListRepository WatchLists { get; }

        public Task<int> CompleteAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);

        public ValueTask DisposeAsync()
        {
            return _context.DisposeAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}