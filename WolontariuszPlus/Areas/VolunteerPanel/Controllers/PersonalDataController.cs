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
    public class PersonalDataController : Controller
    {
        private readonly CMSDbContext _db;

        public PersonalDataController(CMSDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
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
        public IActionResult Index(UserViewModel vm)
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

        public AppUser LoggedUser => _db.AppUsers.First(u => u.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}