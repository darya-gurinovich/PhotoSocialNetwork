using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoSocialNetwork.Models.Storage;
using PhotoSocialNetwork.ViewModels;

namespace PhotoSocialNetwork.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IStorage _storage;

        public ProfileController(IStorage storage)
        {
            _storage = storage;
        }
        public IActionResult Index(int profileId)
        {
            var isAuthenticated = User.Identity.Name != null;
            if (!isAuthenticated && profileId == 0) return RedirectToAction("Login", "Account");

            ProfileModel profileModel;
            if (profileId == 0)
            {
                var userName = User.Identity.Name;
                profileModel = _storage.GetProfileModel(userName);
                ViewBag.isCurrentUser = true;
            }
            else
            {
                profileModel = _storage.GetProfileModelById(profileId);

                var currentUserProfile = _storage.GetProfileModel(User.Identity.Name);

                if (!isAuthenticated || profileModel == null)
                    ViewBag.isCurrentUser = false;
                else
                    ViewBag.isCurrentUser = profileModel.ProfileId == currentUserProfile.ProfileId ? true : false;
            }

            if (profileModel != null)
            {
                ViewBag.areFriends = _storage.CheckIfUsersAreFriends(User.Identity.Name, profileModel.UserId);

                return View(profileModel);
            }

            return NotFound();
        }

    }
}