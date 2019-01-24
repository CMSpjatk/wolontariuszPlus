using System;
using System.Collections.Generic;
using System.IO;
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

namespace WolontariuszPlus.Areas.AdministratorPanelArea.Controllers
{
    [Authorize(Roles = Roles.AdministratorRole)]
    [Area("AdministratorPanelArea")]
    public class AdministratorPanelController : Controller
    {
        private readonly CMSDbContext _db;
        private readonly IFormFilesManagement _formFilesManagement;
        public AppUser LoggedUser => _db.AppUsers.First(u => u.IdentityUserId
                                                       == User.FindFirstValue(ClaimTypes.NameIdentifier));

        public AdministratorPanelController(CMSDbContext db, IFormFilesManagement formFilesManagement)
        {
            _db = db;
            _formFilesManagement = formFilesManagement;
        }

        public IActionResult EventsList()
        {
            //var allEvents =

            var vm = new EventsViewModel
            {
                EventViewModels =
                    _db.Events
                       .Include(e => e.Address)
                       .Include(e => e.Organizer)
                       .AsNoTracking()
                       .Where(e => e.Date >= DateTime.Now.AddHours(8))
                       .ToList()
                       .Select(e => CreateEventViewModelForDisplaying(e)),
                ViewType = PanelViewType.UPCOMING_EVENTS
            };

            return View(vm);
        }

        public IActionResult UpdateEvent(int eventId)
        {
            var eventToUpdate = _db.Events.Include(e => e.Address).FirstOrDefault(e => e.EventId == eventId);
            if (eventToUpdate == null)
            {
                return BadRequest(ErrorMessagesProvider.EventErrors.EventNotExists);
            }

            if (eventToUpdate.Date < DateTime.Now.AddMinutes(1))
            {
                return BadRequest(ErrorMessagesProvider.EventErrors.EventDatePassed);
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
                PostalCode = eventToUpdate.Address.PostalCode,
                ImageRelativePath = eventToUpdate.ImageRelativePath
            });
        }


        [HttpPost]
        public IActionResult UpdateEvent(EventViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (vm.Date < DateTime.Now)
            {
                ModelState.AddModelError("Date", "Data wydarzenia musi być przyszła");
                return View(vm);
            }

            var eventToUpdate = _db.Events.Include(e => e.Address).FirstOrDefault(e => e.EventId == vm.EventId);
            if (eventToUpdate == null)
            {
                return BadRequest(ErrorMessagesProvider.EventErrors.EventNotExists);
            }

            if (eventToUpdate.Date < DateTime.Now.AddMinutes(1))
            {
                return BadRequest(ErrorMessagesProvider.EventErrors.EventDatePassed);
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
                catch (DirectoryNotFoundException) { } // stock files

                relativePath = _formFilesManagement.SaveFileToFileSystem(vm.FormFile, vm.EventId);
            }

            eventToUpdate.UpdateEvent(vm.Name, vm.Date, vm.Description, vm.RequiredPoints, vm.Tags, address, relativePath);
            _db.Events.Update(eventToUpdate);
            _db.SaveChanges();

            return RedirectToAction("EventsList");
        }

        [HttpPost]
        public string DeleteEvent(DeleteEventViewModel vm)
        {
            if (!ModelState.IsValid) return "Błąd";

            var eventToDelete = _db.Events.Find(vm.EventId);
            if (eventToDelete == null)
            {
                return "Błąd";
            }

            if (eventToDelete.Date < DateTime.Now.AddMinutes(1))
            {
                return "Błąd";
            }

            _db.Events.Remove(eventToDelete);
            _db.SaveChanges();

            try
            {
                _formFilesManagement.DeleteWholeEventFolder(eventToDelete.EventId);
            }
            catch (DirectoryNotFoundException) { } // stock files

            return "/AdministratorPanelArea/AdministratorPanel/EventsList";
        }

        public IActionResult PlannedEventDetails(int eventId)
        {
            var ev = _db.Events.Find(eventId);
            if (ev == null)
            {
                return BadRequest(ErrorMessagesProvider.EventErrors.EventNotExists);
            }

            var voes = _db.VolunteersOnEvent
                .Include(voe => voe.Volunteer)
                .Where(voe => voe.EventId == eventId)
                .ToList();

            var volunteers = voes.Select(voe =>
                new VolunteerViewModel
                {
                    VolunteerId = voe.Volunteer.AppUserId,
                    Name = voe.Volunteer.FullName,
                    Email = _db.Users.Find(voe.Volunteer.IdentityUserId).Email,
                    PhoneNumber = voe.Volunteer.PhoneNumber,
                    ReceivedPoints = voe.PointsReceived,
                    CollectedMoney = voe.AmountOfMoneyCollected,
                    VolunteerOnEventId = voe.VolunteerOnEventId
                }
            ).ToList();

            var vm = new EventDetailsViewModel
            {
                EventId = eventId,
                Date = ev.Date,
                Name = ev.Name,
                Address = ev.Address,
                Organizer = ev.Organizer,
                RequiredPoints = ev.RequiredPoints,
                Description = ev.Description,
                Volunteers = volunteers,
                ViewType = PanelViewType.UPCOMING_EVENTS,
                ImageRelativePath = ev.ImageRelativePath
            };

            return View("PlannedEventDetails", vm);
        }

        private DisplayEventViewModel CreateEventViewModelForDisplaying(Event e)
        {
            var n = (e.Address.ApartmentNumber == null) ? "" : ("/" + e.Address.ApartmentNumber.ToString());

            return new DisplayEventViewModel
            {
                EventId = e.EventId,
                Name = e.Name,
                Date = e.Date,
                Address = $"ul. {e.Address.Street} {e.Address.BuildingNumber}{n}, {e.Address.PostalCode} {e.Address.City}",
                ShortenedDescription = e.Description.Length > 100 ? e.Description.Substring(0, 100) + "[...]" : e.Description,
                OrganizerName = e.Organizer.FullName,
                RequiredPoints = e.RequiredPoints
            };
        }
    }
}