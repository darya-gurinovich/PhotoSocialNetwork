using Microsoft.AspNetCore.Mvc;
using PhotoSocialNetwork.Models.Storage;
using PhotoSocialNetwork.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace PhotoSocialNetwork.ViewComponents.Admin
{
    public class UserRegistrationLogsViewComponent : ViewComponent
    {
        private readonly IStorage _storage;

        public UserRegistrationLogsViewComponent(IStorage storage)
        {
            _storage = storage;
        }

        public IViewComponentResult Invoke(string filter)
        {
            return View(_storage.GetRegistrationLogs());
        }
    }
}
