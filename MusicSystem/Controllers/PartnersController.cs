using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Data;
using MusicSystem.Data.Models;
using MusicSystem.Infrastructure;
using MusicSystem.Models.Partners;
using System.Linq;
using static MusicSystem.Constants;

namespace MusicSystem.Controllers
{
    public class PartnersController : Controller
    {
        private readonly MusicSystemDbContext data;

        public PartnersController(MusicSystemDbContext data)
        => this.data = data;

        [Authorize]
        public IActionResult Become() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomePartnerFormModel partner)
        {
            var userId = this.User.GetId();

            var userIsAlreadyPartner = this.data
                .Partners.Any(d => d.UserId == userId);

            if (userIsAlreadyPartner)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(partner);
            }

            var partnerObject = new Partner
            {
                Name = partner.Name,
                BusinessEmail = partner.BusinessEmail,
                Website = partner.Website,
                UserId = userId
            };

            this.data.Partners.Add(partnerObject);
            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "Thank you for becoming a Partner!";

            return RedirectToAction("Add", "Artists");
        }

    }
}