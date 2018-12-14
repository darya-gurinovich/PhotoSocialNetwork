using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSocialNetwork.ViewModels.Logs
{
    public class PostBlockingLogViewModel
    {
        public int PostId { get; set; }
        public DateTime BlockingDate { get; set; }
        public string BlockingStatus { get; set; }
        public string BlockedByUserName { get; set; }

        public PostBlockingLogViewModel(int postId, string blockingStatus, string blockedByUserName, DateTime blockingDate)
        {
            PostId = postId;
            BlockingDate = blockingDate;
            BlockingStatus = blockingStatus;
            BlockedByUserName = blockedByUserName;
        }
    }
}
