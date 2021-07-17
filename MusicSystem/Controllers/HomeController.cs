using Microsoft.AspNetCore.Mvc;
using MusicSystem.Data;
using MusicSystem.Models;
using MusicSystem.Models.Home;
using MusicSystem.Models.Songs;
using System.Diagnostics;
using System.Linq;

namespace MusicSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly MusicSystemDbContext data;

        public HomeController(MusicSystemDbContext data) => this.data = data;

        public IActionResult Index()
        {
            var totalSongs = this.data.Songs.Count();

            var songs = this.data.Songs
            .OrderBy(x => x.Title)
            .Select(x => new SongIndexViewModel
            {
               Id = x.Id,
               Title = x.Title,
               Artist = x.Artist.Name,
               Genre = x.Genre,
               SongUrl = x.SongUrl,
               Lyrics = x.Lyrics,
               Likes = x.Likes
           })
           .Take(10).ToList();

            return View(new IndexViewModel
            {
                TotalSongs = totalSongs,
                Songs = songs
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel 
        { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        
    }
}
