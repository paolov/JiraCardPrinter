using System;

namespace PrintJiraCards.Models
{
    public class SubTask
    {
        internal readonly Issue Issue;
        internal readonly string TicketUrl;

        public SubTask(Issue issue)
        {
            this.Issue = issue;
        }

        public SubTask(Issue issue, string jiraUrl)
        {
            this.Issue = issue;
            this.TicketUrl = string.Format("{0}/browse/{1}", jiraUrl, this.Issue.Key);
        }

        public string Key { get { return this.Issue.Key; } }
        public string IssueType { get { return Issue.Fields.IssueType.Name; } }
        public string Summary { get { return this.Issue.Fields.Summary; } }
        public string Status { get { return this.Issue.Fields.Status.Name; } }
        public User Reporter { get { return this.Issue.Fields.Reporter; } }
        public User Assignee { get { return this.Issue.Fields.Assignee; } }
        public DateTime Created { get { return this.Issue.Fields.Created; } }
        public string HyperLink { get { return this.TicketUrl; } }

        public virtual string ToString(string outputType)
        {
            return base.ToString();
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6}", this.Issue.Fields.IssueType.Name, this.Key,
                                 this.Summary.Replace(",", ""), this.Status,
                                 (this.Assignee != null) ? this.Assignee.Name : "Unassigned", this.Reporter.Name, this.Created);
        }
    }
}