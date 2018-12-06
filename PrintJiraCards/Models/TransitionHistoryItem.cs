using System;

namespace PrintJiraCards.Models
{
    public class TransitionHistoryItem
    {
        public string Type { get; set; }
        public string User { get; set; }
        public DateTime Created { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}