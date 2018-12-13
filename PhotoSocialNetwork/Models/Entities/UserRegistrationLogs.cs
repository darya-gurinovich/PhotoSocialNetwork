using System;
using System.Collections.Generic;

namespace PhotoSocialNetwork.Models
{
    public partial class UserRegistrationLogs
    {
        public int UserRegistrationLogId { get; set; }
        public int UserId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Phone { get; set; }

        public Users User { get; set; }
    }
}
