using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoSocialNetwork.Filters;
using PhotoSocialNetwork.Models.Storage;
using PhotoSocialNetwork.ViewModels;

namespace PhotoSocialNetwork.Controllers
{
    [ServiceFilter(typeof(CheckUserBlockingFilter))]
    public class FeedController : Controller
    {
        private readonly IStorage _storage;

        public FeedController(IStorage storage, IHttpContextAccessor contextAccessor)
        {
            _storage = storage;
        }

        public IActionResult Index()
        {
            return View(_storage.GetUserPosts(User.Identity.Name));
        }

        public IActionResult CreatePost()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult CreatePost(PostViewModel post, IFormFile photo)
        {
            _storage.CreatePost(post, User.Identity.Name, photo);
            return RedirectToAction("Index");
        }
    }
}