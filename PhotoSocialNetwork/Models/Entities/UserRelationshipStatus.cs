using System;
using System.Collections.Generic;

namespace PhotoSocialNetwork.Models
{
    public partial class UserRelationshipStatus
    {
        public UserRelationshipStatus()
        {
            UserRelationships = new HashSet<UserRelationships>();
        }

        public int UserRelationshipStatusId { get; set; }
        public string UserRelationshipStatusName { get; set; }

        public ICollection<UserRelationships> UserRelationships { get; set; }
    }
}
