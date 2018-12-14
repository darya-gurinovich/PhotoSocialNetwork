using PhotoSocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSocialNetwork.ViewModels.Logs
{
    public class UserBlockingLogViewModel
    {
        public string UserName { get; set; }
        public DateTime BlockingDate { get; set; }
        public string BlockingStatus { get; set; }
        public string BlockedByUserName { get; set; }

        public UserBlockingLogViewModel(string userName, string blockingStatus, string blockedByUserName, DateTime blockingDate)
        {
            UserName = userName;
            BlockingDate = blockingDate;
            BlockingStatus = blockingStatus;
            BlockedByUserName = blockedByUserName;
        }
    }
}
