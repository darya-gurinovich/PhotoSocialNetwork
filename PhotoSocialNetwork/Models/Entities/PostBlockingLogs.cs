using System;
using System.Collections.Generic;

namespace PhotoSocialNetwork.Models
{
    public partial class PostBlockingLogs
    {
        public int PostBlockingLogId { get; set; }
        public int BlockingStatusId { get; set; }
        public int BlockedPostId { get; set; }
        public int? BlockedByUserId { get; set; }
        public DateTime BlockingDate { get; set; }

        public Users BlockedByUser { get; set; }
        public Post BlockedPost { get; set; }
        public BlockingStatus BlockingStatus { get; set; }
    }
}
