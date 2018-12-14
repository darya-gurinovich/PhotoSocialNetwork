using Microsoft.AspNetCore.Mvc;
using PhotoSocialNetwork.Models.Storage;
using PhotoSocialNetwork.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace PhotoSocialNetwork.ViewComponents.Feed
{
    public class PostsViewComponent : ViewComponent
    {
        private readonly IStorage _storage;

        public PostsViewComponent(IStorage storage)
        {
            _storage = storage;
        }

        public IViewComponentResult Invoke(int userId)
        {
            return View(_storage.GetUserPosts(userId));
        }
    }
}
