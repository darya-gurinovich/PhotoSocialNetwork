using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSocialNetwork.ViewModels.Account
{
    public class LogInModel
    {
        [Required(ErrorMessage = "The login is required")]
        public string Login { get; set; }

        [Required(ErrorMessage = "The password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
