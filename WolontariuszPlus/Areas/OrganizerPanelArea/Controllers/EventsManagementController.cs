using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WolontariuszPlus.Areas.OrganizerPanelArea.Models;
using WolontariuszPlus.Data;
using WolontariuszPlus.Models;

namespace WolontariuszPlus.Areas.OrganizerPanelArea.Controllers
{
    [Area("OrganizerPanelArea")]
    public class EventsManagementController : Controller
    {
        private readonly CMSDbContext _db;
        public AppUser LoggedUser => _db.AppUsers.First(u => u.IdentityUserId
                                                       == User.FindFirstValue(ClaimTypes.NameIdentifier));


        public EventsManagementController(CMSDbContext db)
        {
            _db = db;
        }


        public IActionResult CreateEvent() => View();


        [HttpPost]
        public IActionResult CreateEvent(EventViewModel vm)
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
            

            var eventToAdd = new Event
            (
                vm.Name,
                vm.Date,
                vm.Description,
                vm.RequiredPoints,
                vm.Tags,
                LoggedUser as Organizer,
                address
            );

            _db.Events.Add(eventToAdd);
            _db.SaveChanges();

            return RedirectToAction("EventsList", "OrganizerPanel");
        }

        public IActionResult UpdateEvent(int eventId)
        {
            var eventToEdit = _db.Events.Find(eventId);
            return View(new EventViewModel
            {
                EventId = eventId,
                Name = eventToEdit.Name,
                Date = eventToEdit.Date,
                Description = eventToEdit.Description,
                RequiredPoints = eventToEdit.RequiredPoints,
                Tags = eventToEdit.Tags,
                City = eventToEdit.Address.City,
                Street = eventToEdit.Address.Street,
                BuildingNumber = eventToEdit.Address.BuildingNumber,
                ApartmentNumber = eventToEdit.Address.ApartmentNumber,
                PostalCode = eventToEdit.Address.PostalCode
            });
        }

        [HttpPost]
        public IActionResult UpdateEvent(EventViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var eventToUpdate = _db.Events.Find(vm.EventId);

            Address address = eventToUpdate.Address;

            if (!(eventToUpdate.Address.City == vm.City && eventToUpdate.Address.Street == vm.Street && eventToUpdate.Address.BuildingNumber == vm.BuildingNumber && eventToUpdate.Address.ApartmentNumber == vm.ApartmentNumber && eventToUpdate.Address.PostalCode == vm.PostalCode))
            {
                address = new Address
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
            }

            eventToUpdate.UpdateEvent(vm.Name, vm.Date, vm.Description, vm.RequiredPoints, vm.Tags, address);
            _db.Events.Update(eventToUpdate);
            _db.SaveChanges();

            return RedirectToAction("EventsList", "OrganizerPanel");
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

            return RedirectToAction("EventsList", "OrganizerPanel");
        }
    }
}