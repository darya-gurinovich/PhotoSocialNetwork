using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoSocialNetwork.Filters;
using PhotoSocialNetwork.Models.Storage;
using PhotoSocialNetwork.ViewModels;

namespace PhotoSocialNetwork.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(CheckUserBlockingFilter))]
    [ServiceFilter(typeof(CheckAdminFilter))]
    public class AdminController : Controller
    {
        private readonly IStorage _storage;

        public AdminController(IStorage storage, IHttpContextAccessor contextAccessor)
        {
            _storage = storage;
        }

        public IActionResult Index(TabViewModel tab)
        {
            if (tab == null || tab.ActiveTab == Tab.Friends)
            {
                tab = new TabViewModel
                {
                    ActiveTab = Tab.UsersPermissions
                };
            }

            return View(tab);
        }

        public IActionResult SwitchTabs(string tabname)
        {
            var tab = new TabViewModel();

            switch (tabname)
            {
                case "UsersPermissions":
                    tab.ActiveTab = Tab.UsersPermissions;
                    break;
                case "UsersBlockings":
                    tab.ActiveTab = Tab.UsersBlockings;
                    break;
                case "PostsBlockings":
                    tab.ActiveTab = Tab.PostsBlockings;
                    break;
            }

            return RedirectToAction("Index", tab);
        }
        
        public IActionResult AddAdminPermission(int userId, int permissionId)
        {
            var responce = _storage.GiveAdminPermission(userId, permissionId);
            return Json(responce);
        }

        public IActionResult RemoveAdminPermission(int userId, int permissionId)
        {
            var responce = _storage.RemoveAdminPermission(userId, permissionId);
            return Json(responce);
        }

        public IActionResult BlockUser(int userId, int statusId)
        {
            var responce = _storage.BlockUser(userId, statusId, User.Identity.Name);
            return Json(responce);
        }

        public IActionResult UnblockUser(int userId)
        {
            var responce = _storage.UnblockUser(userId);
            return Json(responce);
        }

        public IActionResult UsersPermissionsComponent(string filter)
        {
            return ViewComponent("PhotoSocialNetwork.ViewComponents.Admin.UsersPermissions", filter);
        }

        public IActionResult UsersBlockingsComponent(string filter)
        {
            return ViewComponent("PhotoSocialNetwork.ViewComponents.Admin.UsersBlockings", filter);
        }

        public IActionResult PostsBlockingsComponent(string filter)
        {
            return ViewComponent("PhotoSocialNetwork.ViewComponents.Admin.PostsBlockings", filter);
        }

    }
}