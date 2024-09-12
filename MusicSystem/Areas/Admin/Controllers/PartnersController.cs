using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Models.Partners;
using MusicSystem.Services.Partners;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Ban(BanPartnerFormModel partner)
        {
            if (!await this.partners.ExistsAsync(partner.BusinessEmail))
            {
                this.ModelState.AddModelError(nameof(partner.BusinessEmail), "Partner with that Email doesn't exist");
            }

            if (!ModelState.IsValid)
            {
                return View(partner);
            }
            var partnerEmail = partner.BusinessEmail;
            await this.partners.DeleteAsync(partnerEmail);

            TempData[GlobalMessageKey] = "You banned a Partner";

            return RedirectToAction("Manage", "Songs");
        }
    }
}
