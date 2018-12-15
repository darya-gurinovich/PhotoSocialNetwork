using Microsoft.AspNetCore.Mvc;
using PhotoSocialNetwork.Models.Storage;
using PhotoSocialNetwork.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace PhotoSocialNetwork.ViewComponents.Feed
{
    public class CommentsViewComponent : ViewComponent
    {
        private readonly IStorage _storage;

        public CommentsViewComponent(IStorage storage)
        {
            _storage = storage;
        }

        public IViewComponentResult Invoke(int postId)
        {
            return View(_storage.GetPostComments(postId));
        }
    }
}
