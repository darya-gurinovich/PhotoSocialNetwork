using System;
using System.Collections.Generic;

namespace PhotoSocialNetwork.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Text { get; set; }

        public Post Post { get; set; }
        public Users User { get; set; }
    }
}
