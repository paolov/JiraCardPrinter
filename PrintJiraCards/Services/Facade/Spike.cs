using PrintJiraCards.Models;

namespace PrintJiraCards.Services.Facade
{
    public class Spike : Task
    {
        public Spike(Issue issue, string jiraUrl) : base(issue, jiraUrl)
        {
        }

        #region Printer Friendly

        public string Description
        {
            get { return base.Issue.Fields.Description; }
            set { base.Issue.Fields.Description = value; }
        }

        public new string Summary
        {
            get { return base.Summary; }
            set { base.Summary = value; }
        }

        //public string Hyperlink
        //{
        //    get { return base.Hyperlink; }
        //}

        public double Points
        {
            get { return 0; }
        }

        public string Severity
        {
            get { return string.Empty; }
        }

        #endregion Printer Friendly

        #region ToString() Overrides for Reporting

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

        public override string ToString()
        {
            return ToString(string.Empty);
        }

        #endregion ToString() Overrides for Reporting
    }
}