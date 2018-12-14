using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSocialNetwork.ViewModels.Logs
{
    public class RegistrationLogViewModel
    {
        public string UserName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Phone { get; set; }

        public RegistrationLogViewModel(string userName, DateTime registrationDate, string phone)
        {
            UserName = userName;
            RegistrationDate = registrationDate;
            Phone = phone;
        }
    }
}
