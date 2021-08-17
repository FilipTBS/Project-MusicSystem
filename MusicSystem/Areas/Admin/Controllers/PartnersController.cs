using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Models.Partners;
using MusicSystem.Services.Partners;
using static Constants;

namespace MusicSystem.Areas.Admin.Controllers
{
    public class PartnersController : AdminController
    {
        private readonly IPartnerService partners;

        public PartnersController(IPartnerService partners)
        => this.partners = partners;

        public IActionResult Ban()
        {
            return View(new BanPartnerFormModel { });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Ban(BanPartnerFormModel partner)
        {
            if (!this.partners.Exists(partner.BusinessEmail))
            {
                this.ModelState.AddModelError(nameof(partner.BusinessEmail), "Partner with that Email doesn't exist");
            }

            if (!ModelState.IsValid)
            {
                return View(partner);
            }
            var partnerEmail = partner.BusinessEmail;
            this.partners.Delete(partnerEmail);

            TempData[GlobalMessageKey] = "You banned a Partner";

            return RedirectToAction("Manage", "Songs");
        }
    }
}
