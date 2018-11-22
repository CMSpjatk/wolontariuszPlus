using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WolontariuszPlus.Areas.VolunteerPanelArea.Models;
using WolontariuszPlus.Data;
using WolontariuszPlus.Models;

namespace WolontariuszPlus.Areas.VolunteerPanelArea.Controllers
{
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
            var user = LoggedUser;
            var vm = new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                City = user.Address.City,
                Street = user.Address.Street,
                BuildingNumber = user.Address.BuildingNumber,
                ApartmentNumber = user.Address.ApartmentNumber,
                PostalCode = user.Address.PostalCode
            };

            if (user is Volunteer volunteer)
            {
                vm.PESEL = volunteer.PESEL;
                vm.Points = volunteer.Points;
                vm.IsVolunteer = true;
            }
            else if (user is Organizer organizer)
            {
                vm.CreatedEventsCount = organizer.CreatedEventsCount;
            }

            return View(vm);
        }


        [HttpPost]
        public IActionResult PersonalData(UserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = LoggedUser;
            if (user is Volunteer volunteer)
            {
                volunteer.Update(vm.PhoneNumber, vm.City, vm.Street, vm.BuildingNumber, vm.ApartmentNumber, vm.PostalCode, vm.PESEL);
            }
            else
            {
                user.Update(vm.PhoneNumber, vm.City, vm.Street, vm.BuildingNumber, vm.ApartmentNumber, vm.PostalCode);
            }

            _db.AppUsers.Update(user);
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
                       .ToList()
                       .Select(e => CreateEventViewModel(e)),
                ViewType = VolunteerPanelViewType.UPCOMING_EVENTS
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
                       .ToList()
                       .Select(e => CreateEventViewModel(e)),
                ViewType = VolunteerPanelViewType.ARCHIVED_EVENTS
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
                ViewType = VolunteerPanelViewType.EVENTS_WITHOUT_OPINION
            };

            return View("EventsList", vm);
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
                ReceivedPoints = e.PointsReceived
            };
        }

        public AppUser LoggedUser => _db.AppUsers.First(u => u.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}