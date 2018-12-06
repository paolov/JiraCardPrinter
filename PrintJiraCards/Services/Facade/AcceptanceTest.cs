using PrintJiraCards.Models;

namespace PrintJiraCards.Services.Facade
{
    public class AcceptanceTest : Task
    {
        public AcceptanceTest(Issue issue, string jiraUrl) : base(issue, jiraUrl)
        {
            Description = string.Format("Given: {0}{1}When: {2}{3}Then: {4}", this.Given, System.Environment.NewLine, this.When, System.Environment.NewLine, this.Then).Replace(",", "");
        }

        public string Given { get { return base.Issue.Fields.CustomField_10491 ?? string.Empty; } }
        public string When { get { return base.Issue.Fields.CustomField_10492 ?? string.Empty; } }
        public string Then { get { return base.Issue.Fields.CustomField_10493 ?? string.Empty; } }

        #region Printer Friendly

        public string Description { get; set; }

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
            return this.ToString(string.Empty);
        }

        #endregion ToString() Overrides for Reporting
    }
}