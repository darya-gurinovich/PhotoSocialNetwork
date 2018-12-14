using PhotoSocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSocialNetwork.ViewModels
{
    public class PostViewModel
    {
        public int PostId { get; set; }
        public string Text { get; set; }
        public int ProfileId { get; set; }
        public string UserPhotoPath { get; set; }
        public string UserName { get; set; }
        public DateTime PostDate { get; set; }
        public string PhotoPath { get; set; }

        public PostViewModel() { }

        public PostViewModel(Post post, ProfileModel profile)
        {
            PostId = post.PostId;
            ProfileId = profile.ProfileId;
            Text = post.Text;
            UserName = profile.Name;
            UserPhotoPath = profile.PhotoPath;
            PostDate = post.CreationDate;
            PhotoPath = GetPhotoFromByteArray(post.Photo);
        }

        private string GetPhotoFromByteArray(byte[] photoArray)
        {
            if (photoArray == null)
                return "/images/user-profile.jpg";

            var imageBase64Data = Convert.ToBase64String(photoArray);
            var photoPath = string.Format("data:image/png;base64,{0}", imageBase64Data);

            return photoPath;
        }
    }
}
