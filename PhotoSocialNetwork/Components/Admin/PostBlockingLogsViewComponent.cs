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
            return View(_storage.GetPostBlockingLogs());
        }
    }
}
