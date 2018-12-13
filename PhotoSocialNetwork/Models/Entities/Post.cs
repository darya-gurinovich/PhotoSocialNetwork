using System;
using System.Collections.Generic;

namespace PhotoSocialNetwork.Models
{
    public partial class Post
    {
        public Post()
        {
            Comment = new HashSet<Comment>();
            PostBlockingLogs = new HashSet<PostBlockingLogs>();
        }

        public int PostId { get; set; }
        public int UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[] Photo { get; set; }
        public string Text { get; set; }
        public int LikesNumber { get; set; }
        public string Location { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }

        public Users User { get; set; }
        public ICollection<Comment> Comment { get; set; }
        public ICollection<PostBlockingLogs> PostBlockingLogs { get; set; }
    }
}
