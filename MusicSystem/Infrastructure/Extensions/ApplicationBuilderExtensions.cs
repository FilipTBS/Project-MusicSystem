using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicSystem.Data;
using MusicSystem.Data.Models;
using static MusicSystem.Areas.Admin.AdminConstants;

namespace MusicSystem.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            FillFamousArtists(services);
            SeedAdministrator(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<MusicSystemDbContext>();

            data.Database.Migrate();
        }

        private static void FillFamousArtists(IServiceProvider services)
        {
            var data = services.GetRequiredService<MusicSystemDbContext>();
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

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdminRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdminRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "admin@crs.com";
                    const string adminPassword = "admin12";

                    var user = new User
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                        FullName = "Admin"
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
