using PrintJiraCards.Models;
using System.Collections.Generic;
using System.Text;

namespace PrintJiraCards.Services.Facade
{
    /// <summary>
    /// This is a facade for an Issue object of type Story
    /// </summary>
    public class Story : Task
    {
        public Story(Issue issue, string jiraUrl) : base(issue, jiraUrl)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(this.AsA)) sb.AppendFormat("As A: {0}{1}", this.AsA.Replace(",", ""), System.Environment.NewLine);
            if (!string.IsNullOrEmpty(this.When)) sb.AppendFormat("When: {0}{1}", this.When.Replace(",", ""), System.Environment.NewLine);
            if (!string.IsNullOrEmpty(this.IWant)) sb.AppendFormat("I Want: {0}{1}", this.IWant.Replace(",", ""), System.Environment.NewLine);
            if (!string.IsNullOrEmpty(this.SoThat)) sb.AppendFormat("So That: {0}", this.SoThat.Replace(",", ""));
            description = sb.ToString();
            //description = !string.IsNullOrEmpty(this.When)
            //                  ? string.Format("As A: {0}{1}When: {2}{3}I Want: {4}{5}So That: {6}", this.AsA,
            //                                  System.Environment.NewLine, this.When, System.Environment.NewLine,
            //                                  this.IWant, System.Environment.NewLine, this.SoThat).Replace(",", "")
            //                  : string.Format("As A: {0}{1}I Want: {2}{3}So That: {4}", this.AsA,
            //                                  System.Environment.NewLine, this.IWant, System.Environment.NewLine,
            //                                  this.SoThat).Replace(",", "");
        }

        /// <summary>
        /// Custom Fields
        /// </summary>
        public string AsA { get { return base.Issue.Fields.CustomField_10163 ?? string.Empty; } }

        public string When { get { return base.Issue.Fields.CustomField_10492 ?? string.Empty; } }
        public string IWant { get { return base.Issue.Fields.CustomField_10164 ?? string.Empty; } }
        public string SoThat { get { return base.Issue.Fields.CustomField_10165 ?? string.Empty; } }
        public int Iteration { get { return base.Issue.Fields.CustomField_10177; } }
        public double Points { get { return base.Issue.Fields.CustomField_10030; } }
        public List<string> Themes { get { return this.Issue.Fields.CustomField_10175; } }
        public string DeploymentNotes { get { return (string.IsNullOrEmpty(base.Issue.Fields.CustomField_10031)) ? string.Empty : base.Issue.Fields.CustomField_10031.Replace(",", ""); } }

        public string ThemesToString
        {
            get
            {
                if (this.Themes == null) return string.Empty;
                if (this.Themes.Count == 1) return this.Themes[0];
                var themes = string.Empty;
                foreach (var theme in this.Themes)
                {
                    if (!string.IsNullOrEmpty(themes)) themes += "#";
                    themes += theme;
                }
                return themes;
            }
        }

        #region Printer Friendly

        private string description;

        public virtual string Description
        {
            get { return description; }
            set { description = value; }
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
                case "NoAcceptanceCriteria":
                    return string.Format("{0},{1},{2},{3},{4},{5},{6}", this.Key, this.Summary.Replace(",", ""),
                                         this.Project, this.Iteration, this.Status,
                                         (this.Assignee != null) ? this.Assignee.Name : "Unassigned", this.Reporter.Name);

                case "InconsistentStatusResolution":
                    return string.Format("{0},{1},{2},{3},{4},{5},{6}", this.Key, this.Summary.Replace(",", ""),
                                         this.Project, this.Iteration, this.Status,
                                         (string.IsNullOrEmpty(this.Resolution)) ? "Unresolved" : this.Resolution,
                                         this.Reporter.Name);

                case "NoFixVersionStory":
                    return string.Format("{0},{1},{2},{3},{4},{5},{6}", this.Key, this.Summary.Replace(",", ""),
                                         this.Project, this.Status, (string.IsNullOrEmpty(this.Resolution)) ? "Unresolved" : this.Resolution,
                                         (this.Assignee != null) ? this.Assignee.Name : "Unassigned", this.Reporter.Name);

                case "EpicChangeRequestData":
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", this.Issue.Fields.IssueType.Name, this.Key,
                                         this.ThemesToString, this.Project, this.Status,
                                         (string.IsNullOrEmpty(this.Resolution)) ? "Unresolved" : this.Resolution,
                                         this.Reporter.Name, "n/a", this.Feedback);

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