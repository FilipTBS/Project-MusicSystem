using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Data;
using MusicSystem.Infrastructure;
using MusicSystem.Models.Partners;
using MusicSystem.Services.Partners;
using System.Linq;
using static MusicSystem.Constants;

namespace MusicSystem.Controllers
{
    public class PartnersController : Controller
    {
        private readonly IPartnerService partners;

        public PartnersController(IPartnerService partners)
        => this.partners = partners;

        [Authorize]
        public IActionResult Become() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomePartnerFormModel partner)
        {
            var userId = this.User.GetId();

            var userIsAlreadyPartner = this.partners.IsPartner(userId);
                //.Partners.Any(d => d.UserId == userId);

            if (userIsAlreadyPartner)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(partner);
            }

            this.partners.Add(partner.CompanyName, partner.BusinessEmail, partner.Website, userId);

            TempData[GlobalMessageKey] = "Thank you for becoming a Partner!";

            return RedirectToAction("Add", "Artists");
        }

    }
}