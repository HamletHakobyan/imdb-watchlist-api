using System.Linq;
using System.Threading;
using Space.ImdbWatchList.Models.ViewModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Space.ImdbWatchList.Data.Repositories
{
    public class UserRepository
    {
        private readonly ImdbWatchListDbContext _context;

        public UserRepository(ImdbWatchListDbContext context)
        {
            _context = context;
        }

        public async Task<UserVm> GetUserByIdAsync(int id, CancellationToken ct = default)
        {
            var user = await _context.Users.Where(u => u.Id == id).SingleOrDefaultAsync(ct);
            if (user == null)
            {
                return null;
            }

            return new UserVm
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}