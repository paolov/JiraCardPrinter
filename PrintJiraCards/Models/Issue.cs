using System.Collections.Generic;

namespace PrintJiraCards.Models
{
    public class Issue
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public Fields Fields { get; set; }
        public ChangeLog ChangeLog { get; set; }
        public List<Transition> Transitions { get; set; }

        public Issue()
        {
            this.Transitions = new List<Transition>();
        }
    }
}