using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MusicSystem.Data
{
    public class MusicSystemDbContext : IdentityDbContext
    {
        public MusicSystemDbContext(DbContextOptions<MusicSystemDbContext> options)
            : base(options)
        {
        }
    }
}
