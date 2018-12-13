using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoSocialNetwork.Models.Storage;

namespace PhotoSocialNetwork.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IStorage _storage;

        public ProfileController(IStorage storage)
        {
            _storage = storage;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}