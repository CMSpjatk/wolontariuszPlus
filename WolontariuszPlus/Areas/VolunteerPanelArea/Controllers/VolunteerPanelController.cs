using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolontariuszPlus.Areas.VolunteerPanelArea.Models;
using WolontariuszPlus.Data;
using WolontariuszPlus.Models;
using WolontariuszPlus.ViewModels;

namespace WolontariuszPlus.Areas.VolunteerPanelArea.Controllers
{
    [Authorize(Roles = Roles.VolunteerRole)]
    [Area("VolunteerPanelArea")]
    public class VolunteerPanelController : Controller
    {
        private readonly CMSDbContext _db;

        public VolunteerPanelController(CMSDbContext db)
        {
            _db = db;
        }

        public IActionResult PersonalData()
        {
            var volunteer = LoggedUser as Volunteer;
            var vm = new UserViewModel
            {
                FirstName = volunteer.FirstName,
                LastName = volunteer.LastName,
                PhoneNumber = volunteer.PhoneNumber,
                City = volunteer.Address.City,
                Street = volunteer.Address.Street,
                BuildingNumber = volunteer.Address.BuildingNumber,
                ApartmentNumber = volunteer.Address.ApartmentNumber,
                PostalCode = volunteer.Address.PostalCode,
                PESEL = volunteer.PESEL,
                Points = volunteer.Points,
                IsVolunteer = true
            };

            return View(vm);
        }


        [HttpPost]
        public IActionResult PersonalData(UserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var volunteer = LoggedUser as Volunteer;
            volunteer.Update(vm.PhoneNumber, vm.City, vm.Street, vm.BuildingNumber, vm.ApartmentNumber, vm.PostalCode, vm.PESEL);
            

            _db.AppUsers.Update(volunteer);
            _db.SaveChanges();

            return RedirectToAction();
        }

        public IActionResult UpcomingEvents()
        {
            var user = LoggedUser;

            var vm = new EventsViewModel
            {
                EventViewModels = 
                    _db.VolunteersOnEvent
                       .Where(e => e.Event.Date >= DateTime.Now && e.Volunteer == user)
                       .OrderByDescending(e => e.Event.Date)
                       .ToList()
                       .Select(e => CreateEventViewModel(e)),
                ViewType = PanelViewType.UPCOMING_EVENTS
            };
            
            return View("EventsList", vm);
        }


        public IActionResult ArchivedEvents()
        {
            var user = LoggedUser;

            var vm = new EventsViewModel
            {
                EventViewModels =
                    _db.VolunteersOnEvent
                       .Where(e => e.Event.Date < DateTime.Now && e.Volunteer == user)
                       .OrderByDescending(e => e.Event.Date)
                       .ToList()
                       .Select(e => CreateEventViewModel(e)),
                ViewType = PanelViewType.ARCHIVED_EVENTS
            };

            return View("EventsList", vm);
        }


        public IActionResult EventsWithoutOpinion()
        {
            var user = LoggedUser;

            var vm = new EventsViewModel
            {
                EventViewModels =
                    _db.VolunteersOnEvent
                    .Where(voe => voe.Event.Date < DateTime.Now && voe.Volunteer == user && string.IsNullOrEmpty(voe.OpinionAboutEvent))
                    .ToList()
                    .Select(e => CreateEventViewModel(e)),
                ViewType = PanelViewType.EVENTS_WITHOUT_OPINION
            };

            return View("EventsList", vm);
        }

        public IActionResult EventDetails(int eventId, PanelViewType panelViewType)
        {
            var ev = _db.Events.Find(eventId);
            var voes = _db.VolunteersOnEvent.Where(voe => voe.EventId == eventId).ToList();

            var volunteers = voes.Select(voe =>
                new VolunteerViewModel
                {
                    Name = voe.Volunteer.FullName,
                    Email = _db.Users.Find(voe.Volunteer.IdentityUserId).Email,
                    PhoneNumber = voe.Volunteer.PhoneNumber,
                    ReceivedPoints = voe.PointsReceived,
                    CollectedMoney = voe.AmountOfMoneyCollected,
                    VolunteerOnEventId = voe.VolunteerOnEventId,
                    OpinionAboutVolunteer = voe.OpinionAboutVolunteer
                }
            ).ToList();

            var vm = new EventDetailsViewModel
            {
                EventId = eventId,
                Date = ev.Date,
                Name = ev.Name,
                Description = ev.Description,
                CollectedMoneySum = ev.CollectedMoney,
                Volunteers = volunteers,
                ViewType = panelViewType
            };

            return View(vm);
        }

        private EventViewModel CreateEventViewModel(VolunteerOnEvent e)
        {
            var n = (e.Event.Address.ApartmentNumber <= 0) ? " " : ("/" + e.Event.Address.ApartmentNumber.ToString());

            return new EventViewModel
            {
                EventId = e.EventId,
                Name = e.Event.Name,
                Date = e.Event.Date,
                Address = $"ul. {e.Event.Address.Street} {e.Event.Address.BuildingNumber}{n}, {e.Event.Address.PostalCode} {e.Event.Address.City}",
                Description = e.Event.Description,
                OrganizerName = $"{e.Event.Organizer.FirstName} {e.Event.Organizer.LastName}",
                RequiredPoints = e.Event.RequiredPoints,
                ReceivedPoints = e.PointsReceived,
                IsRated = !string.IsNullOrEmpty(e.OpinionAboutEvent) || e.EventRate.HasValue
            };
        }

        public IActionResult AddOpinionAboutEvent(int eventId)
        {
            var vm = new OpinionViewModel
            {
                EventId = eventId
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult AddOpinionAboutEvent(OpinionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUserId = LoggedUser.AppUserId;
            var eventToRateId = vm.EventId;

            var userInEvent = _db.VolunteersOnEvent.First(x => x.EventId == eventToRateId && x.VolunteerId == currentUserId);

            userInEvent.AddOpinion(vm.Opinion, vm.Rate);

            _db.VolunteersOnEvent.Update(userInEvent);
            _db.SaveChanges();

            return RedirectToAction("ArchivedEvents");
        }

        public IActionResult UpdateOpinionAboutEvent(int eventId)
        {
            var currentUserId = LoggedUser.AppUserId;

            var userInEvent = _db.VolunteersOnEvent.First(x => x.EventId == eventId && x.VolunteerId == currentUserId);

            var vm = new OpinionViewModel
            {
                Opinion = userInEvent.OpinionAboutEvent,
                Rate = userInEvent.EventRate,
                EventId = eventId
            };

            return View("AddOpinionAboutEvent", vm);
        }

        public AppUser LoggedUser => _db.AppUsers.First(u => u.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}