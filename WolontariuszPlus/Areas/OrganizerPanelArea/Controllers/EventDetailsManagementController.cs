using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WolontariuszPlus.Areas.OrganizerPanelArea.Models;
using WolontariuszPlus.Areas.OrganizerPanelArea.Models.EventDetailsManagement;
using WolontariuszPlus.Data;
using WolontariuszPlus.Models;

namespace WolontariuszPlus.Areas.OrganizerPanelArea.Controllers
{
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


        public IActionResult PlannedEventDetails(int eventId)
        {
            var ev = _db.Events.Find(eventId);

            var voes = _db.VolunteersOnEvent.Where(voe => voe.EventId == eventId).ToList();

            var volunteers = voes.Select(voe =>
                new VolunteerViewModel
                {
                    Name = voe.Volunteer.FullName,
                    Email = _db.Users.Find(voe.Volunteer.IdentityUserId).Email,
                    PhoneNumber = voe.Volunteer.PhoneNumber,
                    Points = voe.PointsReceived,
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
                CollectedMoneySum = ev.CollectedMoney,
                Volunteers = volunteers,
                ViewType = VolunteerPanelViewType.UPCOMING_EVENTS
            };

            return View("Details", vm);
        }


        public IActionResult PastEventDetails(int eventId)
        {
            var ev = _db.Events.Find(eventId);
            var voes = _db.VolunteersOnEvent.Where(voe => voe.EventId == eventId).ToList();
            var isVolunteer = User.IsInRole(Roles.VolunteerRole);

            var volunteers = voes.Select(voe =>
                new VolunteerViewModel
                {
                    Name = voe.Volunteer.FullName,
                    Email = _db.Users.Find(voe.Volunteer.IdentityUserId).Email,
                    PhoneNumber = voe.Volunteer.PhoneNumber,
                    Points = voe.PointsReceived,
                    CollectedMoney = voe.AmountOfMoneyCollected,
                    VolunteerOnEventId = voe.VolunteerOnEventId,
                    IsRated = !string.IsNullOrEmpty(voe.OpinionAboutVolunteer),
                    OpinionAboutVolunteer = isVolunteer ? voe.OpinionAboutVolunteer : null
                }
            ).ToList();

            var vm = new EventDetailsViewModel
            {
                EventId = eventId,
                Date = ev.Date,
                Name = ev.Name,
                CollectedMoneySum = ev.CollectedMoney,
                Volunteers = volunteers,
                ViewType = VolunteerPanelViewType.ARCHIVED_EVENTS
            };

            return View("Details", vm);
        }


        public IActionResult RateVolunteer(int volunteerOnEventId)
        {
            var voe = _db.VolunteersOnEvent.Find(volunteerOnEventId);

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

            var voe = _db.VolunteersOnEvent.Find(vm.VolunteerOnEventId);

            voe.OpinionAboutVolunteer = vm.RateContent;

            _db.VolunteersOnEvent.Update(voe);
            _db.SaveChanges();

            return RedirectToAction("PastEventDetails", new { eventId = voe.EventId });
        }


        public IActionResult AddMoney(int volunteerOnEventId, VolunteerPanelViewType viewType)
        {
            var voe = _db.VolunteersOnEvent.Find(volunteerOnEventId);

            var vm = new AddMoneyViewModel
            {
                VolunteerOnEventId = voe.VolunteerOnEventId,
                EventId = voe.EventId,
                VolunteerName = voe.Volunteer.FullName,
                EventName = voe.Event.Name,
                ViewType = viewType
            };

            return View(vm);
        }


        [HttpPost]
        public IActionResult AddMoney(AddMoneyViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var voe = _db.VolunteersOnEvent.Find(vm.VolunteerOnEventId);

            voe.AmountOfMoneyCollected = vm.CollectedMoney;

            _db.VolunteersOnEvent.Update(voe);
            _db.SaveChanges();

            if (vm.ViewType == VolunteerPanelViewType.UPCOMING_EVENTS)
            {
                return RedirectToAction("PlannedEventDetails", new { eventId = voe.EventId });
            }
            else
            {
                return RedirectToAction("PastEventDetails", new { eventId = voe.EventId });
            }
        }
        

        public IActionResult RemoveVolunteerFromEvent(int volunteerOnEventId)
        {
            var voe = _db.VolunteersOnEvent.Find(volunteerOnEventId);

            if (voe.Event.Date < DateTime.Now.AddMinutes(1))
            {
                return BadRequest();
            }

            var vm = new RemoveVolunteerViewModel
            {
                VolunteerOnEventId = voe.VolunteerOnEventId,
                EventId = voe.EventId,
                VolunteerName = voe.Volunteer.FullName,
                EventName = voe.Event.Name
            };

            return View(vm);
        }


        [HttpPost]
        public IActionResult RemoveVolunteerFromEvent(RemoveVolunteerViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var voe = _db.VolunteersOnEvent.Find(vm.VolunteerOnEventId);

            if (voe.Event.Date < DateTime.Now.AddMinutes(1))
            {
                return BadRequest();
            }
            
            _db.VolunteersOnEvent.Remove(voe);
            _db.SaveChanges();

            return RedirectToAction("PlannedEventDetails", new { eventId = voe.EventId });
        }
    }
}