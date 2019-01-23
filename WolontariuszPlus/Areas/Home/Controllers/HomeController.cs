using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WolontariuszPlus.Areas.OrganizerPanelArea.Models;
using WolontariuszPlus.Common;
using WolontariuszPlus.Data;
using WolontariuszPlus.Models;
using WolontariuszPlus.ViewModels;

namespace WolontariuszPlus.Areas.Home.Controllers
{
    [AllowAnonymous]
    [Area("Home")]
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
            var displayEventVms = _db.Events
                .Include(e => e.Address)
                .Include(e => e.Organizer)
                .AsNoTracking()
                .Where(e => e.Date >= DateTime.Now.AddHours(8))
                .OrderBy(e => e.Date)
                .Select(e => CreateEventViewModelForDisplaying(e))
                .ToList();

            if (User.Identity.IsAuthenticated && User.IsInRole(Roles.VolunteerRole))
            {
                AppUser loggedUser = LoggedUser;
                ViewBag.VolunteerPoints = ((Volunteer)loggedUser).Points;

                var eventsInWhichThisVolunteerTakesPart = _db.VolunteersOnEvent
                    .Include(voe => voe.Volunteer)
                    .Include(voe => voe.Event)
                    .Where(voe => voe.Volunteer.IdentityUserId == loggedUser.IdentityUserId)
                    .Select(voe => voe.Event)
                    .AsNoTracking()
                    .ToList();

                displayEventVms.ForEach(dvm =>
                    dvm.IsOnEvent = eventsInWhichThisVolunteerTakesPart.Any(cc => cc.EventId == dvm.EventId)
                );
            }

            var vm = new EventsViewModel
            {
                EventViewModels = displayEventVms
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
                ShortenedDescription = e.Description.Length > 100 ? e.Description.Substring(0, 100) + "[...]" : e.Description,
                OrganizerName = $"{e.Organizer.FirstName} {e.Organizer.LastName}",
                RequiredPoints = e.RequiredPoints,
                IsOnEvent = false,
                ImageRelativePath = e.ImageRelativePath
            };
        }

        [Authorize(Roles = Roles.VolunteerRole)]
        public IActionResult AddVolunteerToEvent(int eventId)
        {
            var choosenEvent = _db.Events.Find(eventId);
            if (choosenEvent == null)
            {
                return BadRequest(ErrorMessagesProvider.EventErrors.EventNotExists);
            }
            
            if (!User.IsInRole(Roles.VolunteerRole))
            {
                return BadRequest(ErrorMessagesProvider.EventErrors.OnlyVolunteerCanTakePartInEvent);
            }

            if (choosenEvent.Date <= DateTime.Today)
            {
                return BadRequest(ErrorMessagesProvider.EventErrors.EventDatePassed);
            }
            
            var volunteerToAdd = LoggedUser as Volunteer;

            if (volunteerToAdd.Points < choosenEvent.RequiredPoints)
            {
                return BadRequest(ErrorMessagesProvider.EventErrors.NotEnoughPoints);
            }

            choosenEvent.AddVolunteerToEvent(volunteerToAdd);

            _db.Events.Update(choosenEvent);
            _db.SaveChanges();

            string referrer = Request.Headers["Referer"].ToString();

            if (referrer.Contains("EventDetails"))
            {
                return RedirectToAction("EventDetails", new { eventId = eventId });
            }

            return RedirectToAction("Index");
        }

        public IActionResult EventDetails(int eventId)
        {
            var @event = _db.Events.Find(eventId);
            if (@event == null)
            {
                return BadRequest(ErrorMessagesProvider.EventErrors.EventNotExists);
            }

            if (User.Identity.IsAuthenticated && User.IsInRole(Roles.VolunteerRole))
            {
                AppUser loggedUser = LoggedUser;
                ViewBag.VolunteerPoints = ((Volunteer)loggedUser).Points;
                ViewBag.IsOnEvent = false;
            
                var isOnEvent = _db.VolunteersOnEvent
                    .Include(voe => voe.Volunteer)
                    .Include(voe => voe.Event)
                    .Where(voe => voe.Volunteer.IdentityUserId == loggedUser.IdentityUserId)
                    .Select(voe => voe.Event)
                    .Any(e => e.EventId == eventId);

                ViewBag.IsOnEvent = isOnEvent;
            }

            return View(@event);
        }
    }
}
