using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WolontariuszPlus.Areas.OrganizerPanelArea.Models;
using WolontariuszPlus.Common;
using WolontariuszPlus.Data;
using WolontariuszPlus.Models;

namespace WolontariuszPlus.Areas.OrganizerPanelArea.Controllers
{
    [Area("OrganizerPanelArea")]
    public class EventsManagementController : Controller
    {
        private readonly CMSDbContext _db;
        private readonly IFormFilesManagement _formFilesManagement;
        public AppUser LoggedUser => _db.AppUsers.First(u => u.IdentityUserId
                                                       == User.FindFirstValue(ClaimTypes.NameIdentifier));


        public EventsManagementController(CMSDbContext db, IFormFilesManagement formFilesManagement)
        {
            _db = db;
            _formFilesManagement = formFilesManagement;
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

            if (existingAddress != null)
            {
                address.AddressId = existingAddress.AddressId;
            }

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

            string relativePath = string.Empty;

            if (vm.FormFile == null)
            {
                relativePath = _formFilesManagement.GetPathToRandomStockImage();    
            }
            else
            {
                relativePath = _formFilesManagement.SaveFileToFileSystem(vm.FormFile, eventToAdd.EventId);
            }

            eventToAdd.ImageRelativePath = relativePath;
            _db.SaveChanges();

            return RedirectToAction("EventsList", "OrganizerPanel");
        }


        public IActionResult UpdateEvent(int eventId)
        {
            var eventToUpdate = _db.Events.Find(eventId);

            if (eventToUpdate.Date < DateTime.Now.AddMinutes(1))
            {
                return BadRequest();
            }

            return View(new EventViewModel
            {
                EventId = eventId,
                Name = eventToUpdate.Name,
                Date = eventToUpdate.Date,
                Description = eventToUpdate.Description,
                RequiredPoints = eventToUpdate.RequiredPoints,
                Tags = eventToUpdate.Tags,
                City = eventToUpdate.Address.City,
                Street = eventToUpdate.Address.Street,
                BuildingNumber = eventToUpdate.Address.BuildingNumber,
                ApartmentNumber = eventToUpdate.Address.ApartmentNumber,
                PostalCode = eventToUpdate.Address.PostalCode
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

            if (eventToUpdate.Date < DateTime.Now.AddMinutes(1))
            {
                return BadRequest();
            }

            Address address = null;

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
                    address = existingAddress;
                }
            }

            string relativePath = eventToUpdate.ImageRelativePath;
            if (vm.FormFile != null)
            {
                try
                {
                    _formFilesManagement.DeleteWholeEventFolder(vm.EventId);
                }
                catch (DirectoryNotFoundException exc) {} // stock files

                relativePath = _formFilesManagement.SaveFileToFileSystem(vm.FormFile, vm.EventId);
            }

            eventToUpdate.UpdateEvent(vm.Name, vm.Date, vm.Description, vm.RequiredPoints, vm.Tags, address, relativePath);
            _db.Events.Update(eventToUpdate);
            _db.SaveChanges();

            return RedirectToAction("EventsList", "OrganizerPanel");
        }


        public IActionResult DeleteEvent(int eventId)
        {
            if (eventId <= 0)
            {
                return BadRequest();
            }

            var eventToDelete = _db.Events.Find(eventId);

            if (eventToDelete == null || eventToDelete.Date < DateTime.Now.AddMinutes(1))
            {
                return BadRequest();
            }

            return View(new DeleteEventViewModel { EventId = eventId, Name = eventToDelete.Name });
        }


        [HttpPost]
        public IActionResult DeleteEvent(DeleteEventViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var eventToDelete = _db.Events.Find(vm.EventId);

            if (eventToDelete.Date < DateTime.Now.AddMinutes(1)){
                return BadRequest();
            }

            _db.Events.Remove(eventToDelete);
            _db.SaveChanges();

            _formFilesManagement.DeleteWholeEventFolder(eventToDelete.EventId);

            return RedirectToAction("EventsList", "OrganizerPanel");
        }
    }
}