using System;
using System.Collections.Generic;

namespace PhotoSocialNetwork.Models
{
    public partial class UserPermissions
    {
        public UserPermissions()
        {
            UserAccessRights = new HashSet<UserAccessRights>();
        }

        public int UserPermissionId { get; set; }
        public string PermissionName { get; set; }

        public ICollection<UserAccessRights> UserAccessRights { get; set; }
    }
}
