using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolontariuszPlus.Areas.OrganizerPanelArea.Models;
using WolontariuszPlus.Data;
using WolontariuszPlus.Models;
using WolontariuszPlus.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WolontariuszPlus.Areas.OrganizerPanelArea.Controllers
{
    [Authorize(Roles = Roles.OrganizerRole)]
    [Area("OrganizerPanelArea")]
    public class OrganizerPanelController : Controller
    {
        private readonly CMSDbContext _db;
        public AppUser LoggedUser => _db.AppUsers.First(u => u.IdentityUserId
                                                       == User.FindFirstValue(ClaimTypes.NameIdentifier));

        public OrganizerPanelController(CMSDbContext db)
        {
            _db = db;
        }

        public IActionResult PersonalData()
        {
            var user = LoggedUser as Organizer;
            var vm = new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                City = user.Address.City,
                Street = user.Address.Street,
                BuildingNumber = user.Address.BuildingNumber,
                ApartmentNumber = user.Address.ApartmentNumber,
                PostalCode = user.Address.PostalCode,
                CreatedEventsCount = user.CreatedEventsCount
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

            var organizer = LoggedUser;

            organizer.Update(vm.PhoneNumber, vm.City, vm.Street, vm.BuildingNumber, vm.ApartmentNumber, vm.PostalCode);

            _db.AppUsers.Update(organizer);
            _db.SaveChanges();

            return RedirectToAction();
        }


        public IActionResult EventsList()
        {
            var user = LoggedUser;

            var vm = new EventsViewModel
            {
                EventViewModels =
                    _db.Events
                       .Where(e => e.Date >= DateTime.Now && e.Organizer == user)
                       .ToList()
                       .Select(e => CreateEventViewModelForDisplaying(e)),
                ViewType = VolunteerPanelViewType.UPCOMING_EVENTS
            };

            return View("EventsList", vm);
        }


        public IActionResult PastEvents()
        {
            var user = LoggedUser;

            var vm = new EventsViewModel
            {
                EventViewModels =
                    _db.Events
                       .Where(e => e.Date < DateTime.Now && e.Organizer == user)
                       .ToList()
                       .Select(e => CreateEventViewModelForDisplaying(e)),
                ViewType = VolunteerPanelViewType.ARCHIVED_EVENTS
            };

            return View("EventsList", vm);
        }


        public IActionResult CreateEvent() => View();

        [HttpPost]
        public IActionResult CreateEvent(AddEventViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var address = new Address
            (
                vm.City,
                vm.Street,
                vm.BuildingNumber,
                vm.ApartmentNumber,
                vm.PostalCode
            );

            var existingAddress = _db.Addresses.FirstOrDefault(a =>
                a.City == address.City &&
                a.Street == address.Street &&
                a.BuildingNumber == address.BuildingNumber &&
                a.ApartmentNumber == address.ApartmentNumber &&
                a.PostalCode == address.PostalCode
            );

            if (existingAddress != null)
            {
                address.AddressId = existingAddress.AddressId;
            }

            var eventToAdd = new Event
            (
                vm.Name,
                DateTime.Now,
                vm.Description,
                vm.RequiredPoints,
                vm.Tags,
                LoggedUser as Organizer,
                address
            );

            _db.Events.Add(eventToAdd);
            _db.SaveChanges();

            return RedirectToAction("EventsList");
        }

        public IActionResult DeleteEvent(int eventId)
        {
            var eventToDelete = _db.Events.Find(eventId);
            return View(new DeleteEventViewModel { EventId = eventId, Name = eventToDelete.Name });
        }

        [HttpPost]
        public IActionResult DeleteEvent(DeleteEventViewModel vm)
        {
            var eventToDelete = _db.Events.Find(vm.EventId);
            _db.Events.Remove(eventToDelete);
            _db.SaveChanges();

            return RedirectToAction("EventsList");
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
