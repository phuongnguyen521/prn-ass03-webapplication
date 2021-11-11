using eStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace eStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //consideration
            if (HttpContext.Session.Keys.Any())
            {
                string user = HttpContext.Session.GetString("LoginUserEmail");
                if (user.Equals("Admin"))
                    return RedirectToAction("Index", "Members");
                return RedirectToAction("Profile", "Members");
            }
           return RedirectToAction("Login", "Login");
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult UserDenialPermission()
        {
            return View();
        }
    }
}
