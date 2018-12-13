using System;
using System.Collections.Generic;

namespace PhotoSocialNetwork.Models
{
    public partial class UserRelationships
    {
        public int UserRelationshipId { get; set; }
        public int UserRelationshipStatusId { get; set; }
        public int MainUserId { get; set; }
        public int DependentUserId { get; set; }

        public Users DependentUser { get; set; }
        public Users MainUser { get; set; }
        public UserRelationshipStatus UserRelationshipStatus { get; set; }
    }
}
