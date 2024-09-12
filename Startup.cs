using MusicSystem.Data;
using MusicSystem.Data.Models;
using MusicSystem.Services.Songs;
using MusicSystem.Services.Curators;
using MusicSystem.Services.Statistics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicSystem.Services;
using MusicSystem.Infrastructure.Extensions;
using MusicSystem.Services.Partners;
using MusicSystem.Services.Artists;
using MusicSystem.Services.Feedbacks;

namespace MusicSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration) 
            => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<MusicSystemDbContext>(options => options
                    .UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddDefaultIdentity<User>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MusicSystemDbContext>();

            services.AddAutoMapper(typeof(Startup));

            services.AddMemoryCache();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });

            services.AddTransient<ISongService, SongService>();
            services.AddTransient<ICuratorService, CuratorService>();
            services.AddTransient<IPartnerService, PartnerService>();
            services.AddTransient<IArtistService, ArtistService>();
            services.AddTransient<IStatisticsService, StatisticsService>();
            services.AddTransient<IFeedbackService, FeedbackService>();
            services.AddResponseCompression();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseResponseCompression()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultAreaRoute();

                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();
                });
        }
    }
}
