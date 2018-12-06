using PrintJiraCards.Models;

namespace PrintJiraCards.Services.Facade
{
    public class TechnicalTicket : Story
    {
        public TechnicalTicket(Issue issue, string jiraUrl) : base(issue, jiraUrl)
        {
        }

        public override string Description
        {
            get { return base.Issue.Fields.Description; }
            set { base.Issue.Fields.Description = value; }
        }

        public override string ToString(string outputType)
        {
            switch (outputType)
            {
                case "PrinterFriendly":
                    return string.Format("{0},{1},{2},{3},{4},{5}", this.Key, this.IssueType, this.Status,
                                         this.Resolution, this.Summary.Replace(",", ""),
                                         string.IsNullOrEmpty(this.Description) ? string.Empty : this.Description.Replace(",", ""));

                default:
                    return base.ToString(outputType);
            }
        }
    }
}