using PrintJiraCards.Models;
using System;
using System.Globalization;

namespace PrintJiraCards.Services.Facade
{
    /// <summary>
    /// This is a facade for an Issue object of type Defect
    /// </summary>
    public class Defect : Task
    {
        public Defect(Issue issue, string jiraUrl) : base(issue, jiraUrl)
        {
            Description = string.Format("Observed: {0}{1}Expected: {2}", this.Observed, System.Environment.NewLine, this.Expected).Replace(",", "");
        }

        /// <summary>
        /// Custom Fields
        /// </summary>
        public double Points { get { return base.Issue.Fields.CustomField_10030; } }

        public string Environment { get { return (base.Issue.Fields.CustomField_10180 != null) ? base.Issue.Fields.CustomField_10180.Value : string.Empty; } }
        public string TestPhase { get { return (base.Issue.Fields.CustomField_10490 != null) ? base.Issue.Fields.CustomField_10490.Value : string.Empty; } }
        public string Organisation { get { return this.Issue.Fields.CustomField_10167 ?? string.Empty; } }
        public string Severity { get { return (base.Issue.Fields.CustomField_10151 != null) ? base.Issue.Fields.CustomField_10151.Value : string.Empty; } }
        public string FoundInVersion { get { return (base.Issue.Fields.CustomField_10152 != null) ? base.Issue.Fields.CustomField_10152.Name : string.Empty; } }
        public string DeploymentNotes { get { return (string.IsNullOrEmpty(base.Issue.Fields.CustomField_10031)) ? string.Empty : base.Issue.Fields.CustomField_10031.Replace(",", ""); } }
        public string RootCause { get { return (base.Issue.Fields.CustomField_10086 != null) ? base.Issue.Fields.CustomField_10086.Value : string.Empty; } }
        public string Observed { get { return base.Issue.Fields.CustomField_10173 ?? string.Empty; } }
        public string Expected { get { return base.Issue.Fields.CustomField_10174 ?? string.Empty; } }
        public string StepsToReproduce { get { return base.Issue.Fields.CustomField_10172 ?? string.Empty; } }

        public bool IsLiveIssue
        {
            get { return (Environment.ToUpper().Contains("PROD")); }
        }

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

        #endregion Printer Friendly

        #region ToString() Overrides for Reporting

        public override string ToString(string outputType)
        {
            switch (outputType)
            {
                case "RootCause":
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", this.Key, this.Summary.Replace(",", ""), this.Status,
                                         (string.IsNullOrEmpty(this.Resolution)) ? "Unresolved" : this.Resolution, this.FixVersion,
                                         (this.ReleaseDate != DateTime.MinValue) ? this.ReleaseDate.ToString(CultureInfo.InvariantCulture) : string.Empty,
                                         (this.FirstInPlay != null) ? this.FirstInPlay.User : string.Empty, (this.Reporter != null) ? this.Reporter.Name : "None",
                                         this.FoundInVersion, this.Environment, this.TestPhase, this.Severity);

                case "InconsistentStatusResolution":
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", this.Key, this.Summary.Replace(",", ""), this.Status,
                                         (string.IsNullOrEmpty(this.Resolution)) ? "Unresolved" : this.Resolution, this.FixVersion,
                                         this.FoundInVersion, this.Environment, this.TestPhase, this.Severity, this.Reporter.Name);

                case "DefectDataConsistency":
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", this.Key, this.Status,
                                         (string.IsNullOrEmpty(this.Resolution)) ? "Unresolved" : this.Resolution, this.FixVersion,
                                         (this.ReleaseDate != DateTime.MinValue) ? this.ReleaseDate.ToString(CultureInfo.InvariantCulture) : string.Empty,
                                         this.FoundInVersion, this.Environment, this.TestPhase, this.Severity, this.Reporter.Name, base.Feedback);

                case "NoFixVersionDefect":
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", this.Key, this.Summary.Replace(",", ""), this.Status,
                                         (string.IsNullOrEmpty(this.Resolution)) ? "Unresolved" : this.Resolution,
                                         this.FoundInVersion, this.Environment, this.TestPhase, this.Severity, this.Reporter.Name);

                case "Printable":
                    return string.Format("{0},{1},{2},{3},{4}", this.Key, this.IssueType, this.Status,
                                         this.Summary.Replace(",", ""), this.Description.Replace(",", ""));

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