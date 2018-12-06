namespace PrintJiraCards.Models
{
    public class WorkLogItem
    {
        public User Author { get; set; }
        public int TimeSpentSeconds { get; set; }
    }
}