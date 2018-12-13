using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoSocialNetwork.Models;
using PhotoSocialNetwork.Models.Storage;

namespace PhotoSocialNetwork.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStorage _storage;

        public HomeController(IStorage storage)
        {
            _storage = storage;
        }

        [Authorize]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Profile");
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
