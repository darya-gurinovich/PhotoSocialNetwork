using Microsoft.AspNetCore.Mvc;
using PhotoSocialNetwork.Models.Storage;
using PhotoSocialNetwork.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace PhotoSocialNetwork.ViewComponents.Admin
{
    public class PostsBlockingsViewComponent : ViewComponent
    {
        private readonly IStorage _storage;

        public PostsBlockingsViewComponent(IStorage storage)
        {
            _storage = storage;
        }

        public IViewComponentResult Invoke(string filter)
        {
            return View((_storage.GetPosts(), _storage.GetBlockingStatuses()));
        }
    }
}
