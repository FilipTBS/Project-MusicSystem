using Microsoft.AspNetCore.Mvc;
using MusicSystem.Models;
using MusicSystem.Models.Home;
using System.Diagnostics;
using System.Linq;
using MusicSystem.Services.Songs;
using MusicSystem.Services.Statistics;

namespace MusicSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISongService songs;
        private readonly IStatisticsService statistics;

        public HomeController(ISongService songs, 
            IStatisticsService statistics)
        {
            this.songs = songs;
            this.statistics = statistics;
        }

        public IActionResult Index()
        {
            var latestSongs = this.songs.Latest().ToList();

            var totalStatistics = this.statistics.Total();

            return View(new IndexViewModel
            {
                TotalSongs = totalStatistics.TotalSongs,
                TotalUsers = totalStatistics.TotalUsers,
                Songs = latestSongs
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel 
        { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        
    }
}
