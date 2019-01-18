﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WolontariuszPlus.Areas.OrganizerPanelArea.Models;
using WolontariuszPlus.Areas.OrganizerPanelArea.Models.EventDetailsManagement;
using WolontariuszPlus.Common;
using WolontariuszPlus.Data;
using WolontariuszPlus.Models;
using WolontariuszPlus.ViewModels;

namespace WolontariuszPlus.Areas.OrganizerPanelArea.Controllers
{
    [Authorize(Roles = Roles.OrganizerRole)]
    [Area("OrganizerPanelArea")]
    public class EventDetailsManagementController : Controller
    {
        private readonly CMSDbContext _db;
        public AppUser LoggedUser => _db.AppUsers.First(u => u.IdentityUserId
                                                       == User.FindFirstValue(ClaimTypes.NameIdentifier));


        public EventDetailsManagementController(CMSDbContext db)
        {
            _db = db;
        }


        [Authorize(Roles = Roles.VolunteerRole + ", " + Roles.OrganizerRole)]
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
                Description = ev.Description,
                Volunteers = volunteers,
                ViewType = PanelViewType.UPCOMING_EVENTS,
                ImageRelativePath = ev.ImageRelativePath
            };

            return View("Details", vm);
        }


        public IActionResult PastEventDetails(int eventId)
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
                    Name = voe.Volunteer.FullName,
                    Email = _db.Users.Find(voe.Volunteer.IdentityUserId).Email,
                    PhoneNumber = voe.Volunteer.PhoneNumber,
                    ReceivedPoints = voe.PointsReceived,
                    CollectedMoney = voe.AmountOfMoneyCollected,
                    VolunteerOnEventId = voe.VolunteerOnEventId,
                    IsRated = !string.IsNullOrEmpty(voe.OpinionAboutVolunteer)
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
                ViewType = PanelViewType.ARCHIVED_EVENTS,
                CanModify = ev.CanModify()
            };

            return View("Details", vm);
        }


        public IActionResult RateVolunteer(int volunteerOnEventId)
        {
            var voe = _db.VolunteersOnEvent
                .Include(ve => ve.Volunteer)
                .Include(ve => ve.Event)
                .FirstOrDefault(ve => ve.VolunteerOnEventId == volunteerOnEventId);

            if (voe == null)
            {
                return BadRequest(ErrorMessagesProvider.VolunteerOnEventErrors.VolunteerOnEventNotExists);
            }

            if (!voe.Event.CanModify())
            {
                return BadRequest();
            }

            var vm = new RateVolunteerViewModel
            {
                VolunteerOnEventId = voe.VolunteerOnEventId,
                EventId = voe.EventId,
                VolunteerName = voe.Volunteer.FullName,
                CollectedMoney = voe.AmountOfMoneyCollected,
                Points = voe.PointsReceived,
                EventName = voe.Event.Name,
                RateContent = voe.OpinionAboutVolunteer
            };

            return View(vm);
        }


        [HttpPost]
        public IActionResult RateVolunteer(RateVolunteerViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var voe = _db.VolunteersOnEvent
                .Include(ve => ve.Event)
                .FirstOrDefault(ve => ve.VolunteerOnEventId == vm.VolunteerOnEventId);

            if (voe == null)
            {
                return BadRequest(ErrorMessagesProvider.VolunteerOnEventErrors.VolunteerOnEventNotExists);
            }

            if (!voe.Event.CanModify())
            {
                return BadRequest();
            }

            voe.OpinionAboutVolunteer = vm.RateContent;

            _db.VolunteersOnEvent.Update(voe);
            _db.SaveChanges();

            return RedirectToAction("PastEventDetails", new { eventId = voe.EventId });
        }


        public IActionResult AddMoney(int volunteerOnEventId, double collectedMoney)
        {
            var voe = _db.VolunteersOnEvent
                .Include(ve => ve.Volunteer)
                .Include(ve => ve.Event)
                .FirstOrDefault(ve => ve.VolunteerOnEventId == volunteerOnEventId);

            if (voe == null)
            {
                return BadRequest(ErrorMessagesProvider.VolunteerOnEventErrors.VolunteerOnEventNotExists);
            }

            if (!voe.Event.CanModify())
            {
                return BadRequest();
            }

            var vm = new AddMoneyViewModel
            {
                VolunteerOnEventId = voe.VolunteerOnEventId,
                EventId = voe.EventId,
                VolunteerName = voe.Volunteer.FullName,
                CollectedMoney = collectedMoney,
                EventName = voe.Event.Name
            };

            return View(vm);
        }


        [HttpPost]
        public IActionResult AddMoney(AddMoneyViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var voe = _db.VolunteersOnEvent.Find(vm.VolunteerOnEventId);
            if (voe == null)
            {
                return BadRequest(ErrorMessagesProvider.VolunteerOnEventErrors.VolunteerOnEventNotExists);
            }

            if (!voe.Event.CanModify())
            {
                return BadRequest();
            }

            voe.AmountOfMoneyCollected = vm.CollectedMoney;

            _db.VolunteersOnEvent.Update(voe);
            _db.SaveChanges();

            return RedirectToAction("PastEventDetails", new { eventId = voe.EventId });
        }


        public IActionResult ViewOpinion(int volunteerOnEventId)
        {
            var voe = _db.VolunteersOnEvent.Find(volunteerOnEventId);
            if (voe == null)
            {
                return BadRequest(ErrorMessagesProvider.VolunteerOnEventErrors.VolunteerOnEventNotExists);
            }

            var opinion = new VolunteerPanelArea.Models.OpinionViewModel
            {
                EventId = voe.EventId,
                Opinion = voe.OpinionAboutEvent,
                Rate = voe.EventRate
            };

            return View(opinion);
        }


        [HttpPost]
        public string RemoveVolunteerFromEvent(RemoveVolunteerViewModel vm)
        {
            if (!ModelState.IsValid) return "Błąd";

            var voe = _db.VolunteersOnEvent.Find(vm.VolunteerOnEventId);
            if (voe == null)
            {
                return "Błąd";
            }

            if (voe.Event.Date < DateTime.Now.AddMinutes(1))
            {
                return "Błąd";
            }

            _db.VolunteersOnEvent.Remove(voe);
            _db.SaveChanges();

            return $"/OrganizerPanelArea/EventDetailsManagementController/PlannedEventDetails/{voe.EventId}";
        }

        public IActionResult VolunteerProfile(int userId)
        {
            var voes = _db.VolunteersOnEvent
               .Include(voe => voe.Volunteer)
               .Where(voe => voe.VolunteerOnEventId == userId)
               .Select(x => new UserViewModel
               {
                   FirstName = x.Volunteer.FirstName,
                   LastName = x.Volunteer.LastName,
                   PhoneNumber = x.Volunteer.PhoneNumber,
                   City = x.Volunteer.Address.City,
                   Street = x.Volunteer.Address.Street,
                   BuildingNumber = x.Volunteer.Address.BuildingNumber,
                   ApartmentNumber = x.Volunteer.Address.ApartmentNumber,
                   PostalCode = x.Volunteer.Address.PostalCode,
                   PESEL = x.Volunteer.PESEL,
                   //TODO poprawić coś z lazyloading jesli chodzi o punkty
                   //Points = x.Volunteer.Points,
                   IsVolunteer = true
               }).ToList();

            var model = new UserViewModel
            {
                UserViewModelList = voes
            };

           
            return View(model);
        }
    }
}