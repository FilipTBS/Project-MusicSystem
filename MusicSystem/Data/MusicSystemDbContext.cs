using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicSystem.Data.Models;

namespace MusicSystem.Data
{
    public class MusicSystemDbContext : IdentityDbContext
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Song> Songs { get; set; }     

        public MusicSystemDbContext(DbContextOptions<MusicSystemDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Song>()
            .HasOne(x => x.Artist)
            .WithMany(x => x.Songs)
            .HasForeignKey(x => x.ArtistId);

            base.OnModelCreating(builder);
        }
    }
}
