using System;
using System.Collections.Generic;

namespace PhotoSocialNetwork.Models
{
    public partial class BlockingStatus
    {
        public BlockingStatus()
        {
            PostBlockingLogs = new HashSet<PostBlockingLogs>();
            UserBlockingLogs = new HashSet<UserBlockingLogs>();
        }

        public int BlockingStatusId { get; set; }
        public string BlockingName { get; set; }

        public ICollection<PostBlockingLogs> PostBlockingLogs { get; set; }
        public ICollection<UserBlockingLogs> UserBlockingLogs { get; set; }
    }
}
