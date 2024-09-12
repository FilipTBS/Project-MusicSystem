using Microsoft.AspNetCore.Mvc;
using MusicSystem.Models;
using System.Diagnostics;
using MusicSystem.Services.Songs;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace MusicSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISongService songs;
        private readonly IMemoryCache cache;

        public HomeController(ISongService songs, IMemoryCache cache)
        {
            this.songs = songs;
            this.cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            var exampleSongs = this.cache.Get<IEnumerable<ExampleSongServiceModel>>("ExampleSongsCache");

            if (exampleSongs == null)
            {
                exampleSongs = await this.songs.ExampleAsync();

                var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                await Task.Run(() => this.cache.Set("ExampleSongsCache", exampleSongs, options));
            }       

            return View(exampleSongs);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel 
        { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        
    }
}
