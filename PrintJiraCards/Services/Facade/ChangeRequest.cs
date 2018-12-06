using PrintJiraCards.Models;
using System.Collections.Generic;

namespace PrintJiraCards.Services.Facade
{
    public class ChangeRequest : Task
    {
        public List<string> Themes { get { return this.Issue.Fields.CustomField_10175; } }
        public int Children { get; set; }

        public ChangeRequest(Issue issue, string jiraUrl) : base(issue, jiraUrl)
        {
        }

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

        public override string ToString(string outputType)
        {
            switch (outputType)
            {
                case "EpicChangeRequestData":
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", this.Issue.Fields.IssueType.Name, this.Key,
                                         this.ThemesToString, this.Project, this.Status,
                                         (string.IsNullOrEmpty(this.Resolution)) ? "Unresolved" : this.Resolution,
                                         this.Reporter.Name, this.Children, this.Feedback);

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

        #region Printer Friendly

        public string Description
        {
            get { return this.Issue.Fields.Description; }
            set { this.Issue.Fields.Description = value; }
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
    }
}