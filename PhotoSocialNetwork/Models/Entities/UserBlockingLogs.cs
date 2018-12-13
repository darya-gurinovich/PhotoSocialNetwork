using System;
using System.Collections.Generic;

namespace PhotoSocialNetwork.Models
{
    public partial class UserBlockingLogs
    {
        public int UserBlockingLogId { get; set; }
        public int BlockingStatusId { get; set; }
        public int BlockedUserId { get; set; }
        public int? BlockedByUserId { get; set; }
        public DateTime BlockingDate { get; set; }

        public Users BlockedByUser { get; set; }
        public Users BlockedUser { get; set; }
        public BlockingStatus BlockingStatus { get; set; }
    }
}
