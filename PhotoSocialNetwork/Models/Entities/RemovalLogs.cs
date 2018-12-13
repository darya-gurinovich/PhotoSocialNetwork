using System;
using System.Collections.Generic;

namespace PhotoSocialNetwork.Models
{
    public partial class RemovalLogs
    {
        public int RemovalLogId { get; set; }
        public int? RemovedByUserId { get; set; }
        public DateTime RemovalDate { get; set; }
        public string RemovedObjectInfo { get; set; }

        public Users RemovedByUser { get; set; }
    }
}
