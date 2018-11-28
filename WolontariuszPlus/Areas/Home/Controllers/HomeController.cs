using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolontariuszPlus.Areas.OrganizerPanelArea.Models;
using WolontariuszPlus.Data;
using WolontariuszPlus.Models;
using WolontariuszPlus.ViewModels;

namespace WolontariuszPlus.Areas.Home.Controllers
{
    [Area("Home")]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public AppUser LoggedUser => _db.AppUsers.First(u => u.IdentityUserId
                                                             == User.FindFirstValue(ClaimTypes.NameIdentifier));

        CMSDbContext _db;

        public HomeController(CMSDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var user = LoggedUser;


            var vm = new EventsViewModel
            {
                EventViewModels =
                    _db.Events
                       .Where(e => e.Date >= DateTime.Now && e.Organizer == user)
                       .OrderByDescending(e => e.Date)
                       .Take(5)
                       .ToList()
                       .Select(e => CreateEventViewModelForDisplaying(e)),
                ViewType = VolunteerPanelViewType.UPCOMING_EVENTS
            };

            return View("Index", vm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private DisplayEventViewModel CreateEventViewModelForDisplaying(Event e)
        {
            var n = (e.Address.ApartmentNumber <= 0) ? " " : ("/" + e.Address.ApartmentNumber.ToString());

            return new DisplayEventViewModel
            {
                EventId = e.EventId,
                Name = e.Name,
                Date = e.Date,
                Address = $"ul. {e.Address.Street} {e.Address.BuildingNumber}{n}, {e.Address.PostalCode} {e.Address.City}",
                Description = e.Description,
                OrganizerName = $"{e.Organizer.FirstName} {e.Organizer.LastName}",
                RequiredPoints = e.RequiredPoints
            };
        }
    }
}
