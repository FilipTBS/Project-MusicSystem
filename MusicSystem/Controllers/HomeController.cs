using Microsoft.AspNetCore.Mvc;
using MusicSystem.Models;
using System.Diagnostics;
using System.Linq;
using MusicSystem.Services.Songs;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System;

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

        public IActionResult Index()
        {
            var exampleSongs = this.cache
                .Get<List<ExampleSongServiceModel>>("ExampleSongsCache");
            if (exampleSongs == null)
            {
                exampleSongs = this.songs.Example().ToList();

                var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                this.cache.Set("ExampleSongsCache", exampleSongs, options);
            }       

            return View(exampleSongs);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel 
        { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        
    }
}
