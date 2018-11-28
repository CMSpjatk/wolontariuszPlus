using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WolontariuszPlus.Areas.VolunteerPanelArea.Models;
using WolontariuszPlus.Data;
using WolontariuszPlus.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WolontariuszPlus.Areas.OrganizerPanelArea.Controllers
{
    [Area("OrganizerPanelArea")]
    public class OrganizerPanelController : Controller
    {
        private readonly CMSDbContext _db;

        public OrganizerPanelController(CMSDbContext db)
        {
            _db = db;
        }

        public AppUser LoggedUser => _db.AppUsers.First(u => u.IdentityUserId 
                                                        == User.FindFirstValue(ClaimTypes.NameIdentifier));

        ActionResult PersonalData()
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
    }
}
