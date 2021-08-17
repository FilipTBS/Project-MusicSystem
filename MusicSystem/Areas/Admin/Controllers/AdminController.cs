using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MusicSystem.Areas.Admin.Controllers
{
    using static AdminConstants;
    [Area(AreaName)]
    [Authorize(Roles = AdminRoleName)]
    public abstract class AdminController : Controller
    {
    }
}
