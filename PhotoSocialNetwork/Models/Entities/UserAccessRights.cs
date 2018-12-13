using System;
using System.Collections.Generic;

namespace PhotoSocialNetwork.Models
{
    public partial class UserAccessRights
    {
        public int UserAccessRightId { get; set; }
        public int UserPermissionId { get; set; }
        public int UserId { get; set; }

        public Users User { get; set; }
        public UserPermissions UserPermission { get; set; }
    }
}
