using PrintJiraCards.Models;

namespace PrintJiraCards.Services.Facade
{
    public class Bug : Task
    {
        public Bug(Issue issue, string jiraUrl) : base(issue, jiraUrl)
        {
            Description = string.Format("Observed: {0}{1}Expected: {2}", this.Observed, System.Environment.NewLine, this.Expected).Replace(",", "");
        }

        public string Environment { get { return (base.Issue.Fields.CustomField_10180 != null) ? base.Issue.Fields.CustomField_10180.Value : string.Empty; } }
        public string Observed { get { return base.Issue.Fields.CustomField_10173 ?? string.Empty; } }
        public string Expected { get { return base.Issue.Fields.CustomField_10174 ?? string.Empty; } }

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