using System;
using System.Collections.Generic;

namespace PhotoSocialNetwork.Models
{
    public partial class Profile
    {
        public int ProfileId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte[] Photo { get; set; }

        public Users User { get; set; }
    }
}
