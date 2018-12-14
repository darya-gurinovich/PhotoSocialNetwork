using Microsoft.AspNetCore.Mvc;
using PhotoSocialNetwork.Models.Storage;
using PhotoSocialNetwork.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace PhotoSocialNetwork.ViewComponents.Admin
{
    public class PostBlockingLogsViewComponent : ViewComponent
    {
        private readonly IStorage _storage;

        public PostBlockingLogsViewComponent(IStorage storage)
        {
            _storage = storage;
        }

        public IViewComponentResult Invoke(string filter)
        {
            List<ProfileModel> users;

            var userName = User.Identity.Name;
            if (filter == null || filter == "") {
                if (userName != null)
                    users = _storage.GetAllProfilesWithoutCurrentUser(User.Identity.Name);
                else
                    users = _storage.GetAllProfiles();
            }
            else
            {
                if (userName != null)
                    users = _storage.GetAllProfilesWithoutCurrentUserWithFilter(User.Identity.Name, filter);
                else
                    users = _storage.GetAllProfilesWithFilter(filter);
            }

            return View(users);
        }
    }
}
