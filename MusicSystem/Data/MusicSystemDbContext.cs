using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicSystem.Data.Models;

namespace MusicSystem.Data
{
    public class MusicSystemDbContext : IdentityDbContext<User>
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Curator> Curators { get; set; }
        public DbSet<Partner> Partners { get; set; }

        public MusicSystemDbContext(DbContextOptions<MusicSystemDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Song>()
            .HasOne(x => x.Artist)
            .WithMany(x => x.Songs)
            .HasForeignKey(x => x.ArtistId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Song>()
                .HasOne(x => x.Curator)
                .WithMany(x => x.Songs)
                .HasForeignKey(x => x.CuratorId)
                .OnDelete(DeleteBehavior.Restrict);

                builder
                .Entity<Curator>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Curator>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Artist>()
           .HasMany(x => x.Songs)
           .WithOne(x => x.Artist)
           .HasForeignKey(x => x.ArtistId)
           .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }
    }
}
