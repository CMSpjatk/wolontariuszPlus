using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WolontariuszPlus.Areas.VolunteerPanelArea.Models;
using WolontariuszPlus.Common;
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
            var volunteer = _db.AppUsers
                .Include(u => u.Address)
                .First(u => u.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)) as Volunteer;

            var vm = new UserViewModel
            {
                Email = volunteer.Email,
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
            volunteer.Update(vm.PhoneNumber, vm.City, vm.Street, vm.BuildingNumber, vm.ApartmentNumber, vm.PostalCode);
            
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
                       .Include(voe => voe.Event)
                            .ThenInclude(e => e.Address)
                       .Include(voe => voe.Event)
                            .ThenInclude(e => e.Organizer)
                       .Include(voe => voe.Volunteer)
                       .AsNoTracking()
                       .Where(voe => voe.Event.Date >= DateTime.Now && voe.Volunteer == user)
                       .OrderByDescending(voe => voe.Event.Date)
                       .ToList()
                       .Select(voe => CreateEventViewModel(voe)),
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
                       .Include(voe => voe.Event)
                            .ThenInclude(e => e.Address)
                       .Include(voe => voe.Event)
                            .ThenInclude(e => e.Organizer)
                       .Include(voe => voe.Volunteer)
                       .AsNoTracking()
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
                       .Include(voe => voe.Event)
                            .ThenInclude(e => e.Address)
                       .Include(voe => voe.Event)
                            .ThenInclude(e => e.Organizer)
                       .Include(voe => voe.Volunteer)
                       .AsNoTracking()
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
            if (ev == null)
            {
                return BadRequest(ErrorMessagesProvider.EventErrors.EventNotExists);
            }

            var voes = _db.VolunteersOnEvent
                .Include(voe => voe.Volunteer)
                .Where(voe => voe.EventId == eventId).ToList();

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
                ViewType = panelViewType,
                ImageRelativePath = ev.ImageRelativePath
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

            var userInEvent = _db.VolunteersOnEvent.FirstOrDefault(x => x.EventId == eventToRateId && x.VolunteerId == currentUserId);
            if (userInEvent == null)
            {
                return BadRequest(ErrorMessagesProvider.VolunteerOnEventErrors.VolunteerIsNotOnThisEvent);
            }

            userInEvent.AddOpinion(vm.Opinion, vm.Rate);

            _db.VolunteersOnEvent.Update(userInEvent);
            _db.SaveChanges();

            return RedirectToAction("ArchivedEvents");
        }

        public IActionResult UpdateOpinionAboutEvent(int eventId)
        {
            var currentUserId = LoggedUser.AppUserId;

            var userInEvent = _db.VolunteersOnEvent.First(x => x.EventId == eventId && x.VolunteerId == currentUserId);
            if (userInEvent == null)
            {
                return BadRequest(ErrorMessagesProvider.VolunteerOnEventErrors.VolunteerIsNotOnThisEvent);
            }

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