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
                    ActiveTab = Tab.Users
                };
            }

            return View(tab);
        }

        public IActionResult SwitchTabs(string tabname)
        {
            var tab = new TabViewModel();

            switch (tabname)
            {
                case "Users":
                    tab.ActiveTab = Tab.Users;
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

        public IActionResult UsersComponent(string filter)
        {
            return ViewComponent("PhotoSocialNetwork.ViewComponents.Admin.Users", filter);
        }

    }
}