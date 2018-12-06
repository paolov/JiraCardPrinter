using PrintJiraCards.Models;
using System.Collections.Generic;

namespace PrintJiraCards.Services
{
    public class Search
    {
        public int StartAt { get; set; }
        public int MaxResults { get; set; }
        public int Total { get; set; }
        public List<Issue> Issues { get; set; }

        public Search()
        {
            this.Issues = new List<Issue>();
        }
    }
}