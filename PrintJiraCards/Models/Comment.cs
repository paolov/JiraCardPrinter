using System.Collections.Generic;

namespace PrintJiraCards.Models
{
    public class Comment
    {
        public int Total { get; set; }
        public List<CommentItem> Comments { get; set; }

        public Comment()
        {
            this.Comments = new List<CommentItem>();
        }
    }
}