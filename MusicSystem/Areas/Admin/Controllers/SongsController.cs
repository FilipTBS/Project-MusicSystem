namespace MusicSystem.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MusicSystem.Services.Songs;

    public class SongsController : AdminController
    {
        private readonly ISongService songs;

        public SongsController(ISongService songs)
        => this.songs = songs;
        
        public IActionResult All() 
            => View(this.songs.All(approvedOnly: false).Songs);

        public IActionResult ChangeVisibility(string id)
        {
            this.songs.ChangeVisility(id);

            return RedirectToAction(nameof(All));
        }
    }
}
