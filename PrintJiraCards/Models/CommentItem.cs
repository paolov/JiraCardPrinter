using System;

namespace PrintJiraCards.Models
{
    public class CommentItem
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
    }
}