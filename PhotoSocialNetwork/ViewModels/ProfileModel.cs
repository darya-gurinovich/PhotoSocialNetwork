using PhotoSocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSocialNetwork.ViewModels
{
    public class ProfileModel
    {
        public int ProfileId { get; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [Required(ErrorMessage = "The name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The date of birth is required")]
        public DateTime DateOfBirth { get; set; }
        public string PhotoPath { get; set; }
        public int UserId { get; set; }

        public ProfileModel() { }

        public ProfileModel(Profile profile)
        {
            ProfileId = profile.ProfileId;
            Name = profile.Name;
            DateOfBirth = profile.DateOfBirth;
            PhotoPath = GetPhotoFromByteArray(profile.Photo);
            UserId = profile.UserId;
        }

        public ProfileModel(Profile profile, string email, string phone)
        {
            ProfileId = profile.ProfileId;
            Name = profile.Name;
            DateOfBirth = profile.DateOfBirth;
            PhotoPath = GetPhotoFromByteArray(profile.Photo);
            Email = email;
            UserId = profile.UserId;
            Phone = phone;
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
