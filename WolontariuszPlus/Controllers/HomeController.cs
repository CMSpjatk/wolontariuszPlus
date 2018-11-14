using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WolontariuszPlus.Data;
using WolontariuszPlus.Models;

namespace WolontariuszPlus.Controllers
{
    public class HomeController : Controller
    {
        CMSDbContext _db;

        public HomeController(CMSDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Tests.ToList());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
