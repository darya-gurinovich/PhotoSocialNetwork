using System;
using System.Collections.Generic;

namespace PhotoSocialNetwork.Models
{
    public partial class Users
    {
        public Users()
        {
            Comment = new HashSet<Comment>();
            Post = new HashSet<Post>();
            PostBlockingLogs = new HashSet<PostBlockingLogs>();
            Profile = new HashSet<Profile>();
            RemovalLogs = new HashSet<RemovalLogs>();
            UserAccessRights = new HashSet<UserAccessRights>();
            UserBlockingLogsBlockedByUser = new HashSet<UserBlockingLogs>();
            UserBlockingLogsBlockedUser = new HashSet<UserBlockingLogs>();
            UserRegistrationLogs = new HashSet<UserRegistrationLogs>();
            UserRelationshipsDependentUser = new HashSet<UserRelationships>();
            UserRelationshipsMainUser = new HashSet<UserRelationships>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastLogInDate { get; set; }

        public ICollection<Comment> Comment { get; set; }
        public ICollection<Post> Post { get; set; }
        public ICollection<PostBlockingLogs> PostBlockingLogs { get; set; }
        public ICollection<Profile> Profile { get; set; }
        public ICollection<RemovalLogs> RemovalLogs { get; set; }
        public ICollection<UserAccessRights> UserAccessRights { get; set; }
        public ICollection<UserBlockingLogs> UserBlockingLogsBlockedByUser { get; set; }
        public ICollection<UserBlockingLogs> UserBlockingLogsBlockedUser { get; set; }
        public ICollection<UserRegistrationLogs> UserRegistrationLogs { get; set; }
        public ICollection<UserRelationships> UserRelationshipsDependentUser { get; set; }
        public ICollection<UserRelationships> UserRelationshipsMainUser { get; set; }
    }
}
