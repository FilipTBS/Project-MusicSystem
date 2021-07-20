using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicSystem.Data;
using MusicSystem.Data.Models;

namespace MusicSystem.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider
                      .GetService<MusicSystemDbContext>();

            data.Database.Migrate();

            FillFamousArtists(data);

            return app;
        }

        private static void FillFamousArtists(MusicSystemDbContext data)
        {
                if (data.Artists.Any())
                {
                    return;
                }

                data.Artists.AddRange(new[]
                {
                new Artist { Name = "FYRE", Genre = "Rap/Hiphop"},
                new Artist { Name = "Gery-Nikol", Genre = "Pop"},
                new Artist { Name = "Tita" , Genre = "Pop/Chalga"},
                new Artist { Name = "Krisko", Genre = "Rap/Chalga"},
                new Artist { Name = "Papi Hans" , Genre = "Pop"},
                new Artist { Name = "Pavell & Venci Venc" , Genre = "Pop"},
                new Artist { Name = "Ico Hazarta", Genre = "Rap"},
                new Artist { Name = "Slavi Trifonov", Genre = "Chalga"},
                new Artist { Name = "F.O.", Genre = "Rap"},
                });

                data.SaveChanges();
        }
    }
}
