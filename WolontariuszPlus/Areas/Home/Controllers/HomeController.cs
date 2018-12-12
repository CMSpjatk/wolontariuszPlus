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
        private readonly CMSDbContext _db;
        private AppUser LoggedUser => _db.AppUsers.FirstOrDefault(u => u.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier));

        public HomeController(CMSDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var vm = new EventsViewModel
            {
                EventViewModels =
                    _db.Events
                       .Where(e => e.Date >= DateTime.Now)
                       .OrderByDescending(e => e.Date)
                       .ToList()
                       .Select(e => CreateEventViewModelForDisplaying(e)),
            };

            return View(vm);
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
                ShortenedDescription = e.Description,
                OrganizerName = $"{e.Organizer.FirstName} {e.Organizer.LastName}",
                RequiredPoints = e.RequiredPoints,
                IsOnEvent = LoggedUser != null ? IsVolunteerOnEvent(e) : false
            };
        }

        private bool IsVolunteerOnEvent(Event e)
        {
            bool isOnEvent = false;
            var volunteer = LoggedUser as Volunteer;

            if ((e.VolunteersOnEvent.Any(x => x.Volunteer == volunteer && x.Event == e)))
            {
                isOnEvent = true;
            }

            return isOnEvent;
        }

        [Authorize(Roles = Roles.VolunteerRole)]
        public IActionResult AddVolunteerToEvent(int eventId)
        {
            var choosenEvent = _db.Events.Find(eventId);

            if (choosenEvent.Date <= DateTime.Today)
            {
                return BadRequest("Nie można zapisać się na wydarzenie, które już się odbyło.");
            }
            
            var volunteerToAdd = LoggedUser as Volunteer;

            if (volunteerToAdd.Points < choosenEvent.RequiredPoints)
            {
                return BadRequest("Wolontariusz nie ma wystarczającej liczby punktów, aby zapisać się na to wydarzenie");
            }

            choosenEvent.AddVolunteerToEvent(volunteerToAdd);

            _db.Events.Update(choosenEvent);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
