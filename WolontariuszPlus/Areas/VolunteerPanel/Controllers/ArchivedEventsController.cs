using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WolontariuszPlus.Areas.VolunteerPanel.Models;
using WolontariuszPlus.Data;
using WolontariuszPlus.Models;

namespace WolontariuszPlus.Areas.VolunteerPanel.Controllers
{
    [Area("VolunteerPanel")]
    public class ArchivedEventsController : Controller
    {
        private readonly CMSDbContext _db;

        public ArchivedEventsController(CMSDbContext _db)
        {
            this._db = _db;
        }

        public IActionResult Index()
        {
            var user = LoggedUser;

            var vm = _db.VolunteersOnEvent.Where(e => e.Event.Date < DateTime.Now && e.Volunteer == user)
                .ToList()
                .Select(e => 
                {
                    var n = (e.Event.Address.ApartmentNumber <= 0) ? " " : ("/" + e.Event.Address.ApartmentNumber.ToString());

                    return new ArchivedEventViewModel
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
                }).ToList();

            return View(vm);
        }
        public AppUser LoggedUser => _db.AppUsers.First(u => u.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}