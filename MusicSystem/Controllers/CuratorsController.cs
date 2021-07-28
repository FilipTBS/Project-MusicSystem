﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Data;
using MusicSystem.Infrastructure;
using MusicSystem.Models.Curators;
using System.Linq;

namespace MusicSystem.Controllers
{
    public class CuratorsController : Controller
    {
        private readonly MusicSystemDbContext data;

        public CuratorsController(MusicSystemDbContext data)
        => this.data = data;

        [Authorize]
        public IActionResult Become() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeCuratorFormModel curator)
        {
            var userId = this.User.GetId();

            var userIsAlreadyCurator = this.data.Curators
                .Any(x => x.UserId == userId);

            if (userIsAlreadyCurator)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(curator);
            }

            var curatorObject = new Curator
            {
                Name = curator.Name,
                PhoneNumber = curator.PhoneNumber,
                UserId = userId
            };

            this.data.Curators.Add(curatorObject);
            this.data.SaveChanges();

            return RedirectToAction("All", "Songs");
        }

    }
}