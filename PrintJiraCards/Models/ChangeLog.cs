using System.Collections.Generic;

namespace PrintJiraCards.Models
{
    public class ChangeLog
    {
        public int Total { get; set; }
        public List<History> Histories { get; set; }

        public ChangeLog()
        {
            this.Histories = new List<History>();
        }
    }
}