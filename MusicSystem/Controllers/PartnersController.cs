using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Infrastructure.Extensions;
using MusicSystem.Models.Partners;
using MusicSystem.Services.Partners;
using static Constants;

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

            var email = partner.BusinessEmail;
            var sameEmailAlreadyRegistered = this.partners.CheckForSameEmail(email);

            if (sameEmailAlreadyRegistered)
            {
                TempData[GlobalMessageKey] = "That business email is used by another Partner!";
                return View(partner);
            }

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