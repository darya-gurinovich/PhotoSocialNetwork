using Microsoft.AspNetCore.Mvc;
using PhotoSocialNetwork.Models.Storage;
using PhotoSocialNetwork.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace FilmShare.ViewComponents.Search
{
    public class FriendsViewComponent : ViewComponent
    {
        private readonly IStorage _storage;

        public FriendsViewComponent(IStorage storage)
        {
            _storage = storage;
        }

        public IViewComponentResult Invoke(string filter)
        {
            List<ProfileModel> users;

            if (filter == null || filter.StartsWith(""))
                users = _storage.GetAllProfileModelsWithoutFriends(User.Identity.Name);

            else
            {
                users = _storage.GetProfileModelsWithoutFriendsWithFilter(User.Identity.Name, filter);
            }

            return View(users);
        }
    }
}
