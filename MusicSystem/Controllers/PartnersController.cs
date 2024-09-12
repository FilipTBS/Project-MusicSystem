using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Infrastructure.Extensions;
using MusicSystem.Models.Partners;
using MusicSystem.Services.Partners;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Become(BecomePartnerFormModel partner)
        {
            var userId = this.User.GetId();

            var userIsAlreadyPartner = this.partners.IsPartnerAsync(userId);

            var email = partner.BusinessEmail;
            var sameEmailAlreadyRegistered = this.partners.CheckForSameEmailAsync(email);

            if (await sameEmailAlreadyRegistered)
            {
                TempData[GlobalMessageKey] = "That business email is used by another Partner!";
                return View(partner);
            }

            if (await userIsAlreadyPartner)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(partner);
            }

            await this.partners.AddAsync(partner.CompanyName, partner.BusinessEmail, partner.Website, userId);

            TempData[GlobalMessageKey] = "Thank you for becoming a Partner!";

            return RedirectToAction("Add", "Artists");
        }

    }
}