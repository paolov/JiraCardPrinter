using System;
using System.Collections.Generic;

namespace PrintJiraCards.Models
{
    public class History
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public DateTime Created { get; set; }
        public List<HistoryItem> Items { get; set; }

        public History()
        {
            this.Items = new List<HistoryItem>();
        }
    }
}