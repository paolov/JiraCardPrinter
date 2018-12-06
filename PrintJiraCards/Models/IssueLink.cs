namespace PrintJiraCards.Models
{
    public class IssueLink
    {
        public int Id { get; set; }
        public IssueLinkType Type { get; set; }
        public Issue InwardIssue { get; set; }
    }
}