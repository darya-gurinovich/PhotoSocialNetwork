using PhotoSocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSocialNetwork.ViewModels
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public int ProfileId { get; set; }
        public string UserPhotoPath { get; set; }
        public string UserName { get; set; }
        public DateTime CommentDate { get; set; }
        public string PhotoPath { get; set; }

        public CommentViewModel() { }

        public CommentViewModel(Comment comment, ProfileModel profile)
        {
            ProfileId = profile.ProfileId;
            Text = comment.Text;
            UserName = profile.Name;
            UserPhotoPath = profile.PhotoPath;
            CommentDate = comment.CreationDate;
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
