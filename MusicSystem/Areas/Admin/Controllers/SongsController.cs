namespace MusicSystem.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class SongsController : AdminController
    {
        public IActionResult Index() => View();
    }
}
