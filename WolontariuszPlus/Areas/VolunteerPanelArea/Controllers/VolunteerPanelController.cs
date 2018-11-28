﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WolontariuszPlus.Areas.VolunteerPanelArea.Models;
using WolontariuszPlus.Data;
using WolontariuszPlus.Models;
using WolontariuszPlus.ViewModels;

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