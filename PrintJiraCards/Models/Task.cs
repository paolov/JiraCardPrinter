using System;
using System.Collections.Generic;
using System.Linq;

namespace PrintJiraCards.Models
{
    public class Task
    {
        internal readonly Issue Issue;

        public readonly string TicketUrl;

        public Task()
        {
        }

        protected Task(Issue issue, string jiraUrl)
        {
            this.Issue = issue;
            this.TicketUrl = string.Format("{0}/browse/{1}", jiraUrl, this.Issue.Key);
        }

        public string Key { get { return this.Issue.Key; } }
        public string Hyperlink { get { return this.TicketUrl; } }
        public string Summary { get { return this.Issue.Fields.Summary; } internal set { this.Issue.Fields.Summary = value; } }

        public string SummarySnippet
        {
            get
            {
                return this.Summary.Length > 100 ? string.Format("{0} ...", this.Summary.Substring(0, 96).Replace(",", "")) : this.Summary.Replace(",", "");
            }
        }

        public string IssueType { get { return this.Issue.Fields.IssueType.Name; } }
        public IssueType IssueTypeObj { get { return this.Issue.Fields.IssueType; } }
        public TimeTracking TimeTracking { get { return this.Issue.Fields.Timetracking; } }

        public string EpicKey { get { return this.Issue.Fields.Customfield_10013; } }

        public Parent Parent { get { return this.Issue.Fields.Parent; } }

        public User Reporter { get { return this.Issue.Fields.Reporter; } }
        public User Assignee { get { return this.Issue.Fields.Assignee; } set { this.Issue.Fields.Assignee = value; } }
        public string Project { get { return (this.Issue.Fields.CustomField_10496 != null) ? this.Issue.Fields.CustomField_10496.Value : string.Empty; } }
        public string Status { get { return this.Issue.Fields.Status.Name; } set { this.Issue.Fields.Status.Name = value; } }
        public string Resolution { get { return (this.Issue.Fields.Resolution != null) ? this.Issue.Fields.Resolution.Name : "Unresolved"; } }
        public DateTime Created { get { return this.Issue.Fields.Created; } }
        public string FixVersion { get { return (this.Issue.Fields.FixVersions.Count != 0) ? this.Issue.Fields.FixVersions[0].Name : string.Empty; } }
        public string ProductVersion { get { return (this.Issue.Fields.CustomField_10990 != null) ? this.Issue.Fields.CustomField_10990.Name : string.Empty; } }
        public DateTime ReleaseDate { get { return this.Issue.Fields.CustomField_10176; } }
        public List<IssueLink> IssueLinks { get { return this.Issue.Fields.IssueLinks; } }
        public List<SubTask> SubTasks { get; set; }
        public List<TransitionHistoryItem> TransitionHistory { get; set; }
        public List<Transition> TransitionsAvailable { get { return this.Issue.Transitions; } }
        public List<string> Labels { get { return this.Issue.Fields.Labels; } }
        public string Pod { get { return (this.Issue.Fields.CustomField_11092 != null) ? this.Issue.Fields.CustomField_11092.Value : string.Empty; } }
        public string Sprint { get { return (this.Issue.Fields.CustomField_11091 != null) ? this.Issue.Fields.CustomField_11091.Value : string.Empty; } }
        public string SprintDay { get { return (this.Issue.Fields.CustomField_11190 != null) ? this.Issue.Fields.CustomField_11190.Value : string.Empty; } }

        public string AssigneeName
        {
            get { return Assignee != null ? Assignee.Name : string.Empty; }
        }

        public string Feedback { get; set; }
        public bool Transitioned { get; set; }

        public List<SubTask> Bugs
        {
            get
            {
                return this.SubTasks == null ? null : this.SubTasks.Where(t => t.IssueType.Equals("Bug", StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        public List<SubTask> AcceptanceTests
        {
            get
            {
                return this.SubTasks == null ? null : this.SubTasks.Where(t => t.IssueType.Equals("Acceptance Test", StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        public string ParentKey { get { return (this.Issue.Fields.Parent != null) ? this.Issue.Fields.Parent.Key : string.Empty; } }

        /// <summary>
        /// Finds the last developer to work on a ticket. Calculated looking at the last transition to 'In Play' in the ticket.
        /// This only works if JIRA is used properly.
        /// </summary>
        public string LastDeveloper
        {
            get
            {
                var developer = string.Empty;
                var fromStatuses = new List<string> { "Ready For Dev", "Patched", "Awaiting Bug Fix", "Bug Open" };
                var transitions = FindStatusTransitions(fromStatuses, "In Play");
                if (transitions != null && transitions.Count > 0)
                {
                    // Last one? It is if they are in date order. Transitions are in ascending order
                    developer = transitions[transitions.Count - 1].User;
                }
                return developer;
            }
        }

        /// <summary>
        /// Finds the first developer to work on a ticket. Calculated looking at the last transition to 'In Play' in the ticket.
        /// This only works if JIRA is used properly.
        /// </summary>
        public string FirstDeveloper
        {
            get
            {
                var developer = string.Empty;
                var fromStatuses = new List<string> { "Ready For Dev", "Patched", "Awaiting Bug Fix", "Bug Open" };
                var transitions = FindStatusTransitions(fromStatuses, "In Play");
                if (transitions != null && transitions.Count > 0)
                {
                    // Last one? It is if they are in date order. Transitions are in ascending order
                    developer = transitions[0].User;
                }
                return developer;
            }
        }

        /// <summary>
        /// The first transition of this ticket to 'In Play'.
        /// </summary>
        public TransitionHistoryItem FirstInPlay
        {
            get
            {
                var firstInPlayTransition = this.FindStatusTransitionTo("In Play");
                return firstInPlayTransition ?? null;
            }
        }

        /// <summary>
        /// A list of all JIRA users who put the ticket 'In Play'.
        /// </summary>
        public string InPlayBy
        {
            get
            {
                var inPlayTransitions = this.FindStatusTransitionsTo("In Play");
                if (inPlayTransitions == null) return string.Empty;
                var developers = string.Empty;
                var users = new List<string>();
                foreach (var transition in inPlayTransitions)
                {
                    if (users.Count > 0)
                    {
                        var found = users.Find(t => t == transition.User);
                        if (found == null)
                        {
                            users.Add(transition.User);
                        }
                    }
                    else
                    {
                        users.Add(transition.User);
                    }
                }
                foreach (var user in users)
                {
                    if (!string.IsNullOrEmpty(developers)) developers += "#";
                    developers += user;
                }
                return developers;
            }
        }

        /// <summary>
        /// Calculates the date when a ticket entered a status.
        /// </summary>
        /// <param name="status">The status, e.g. "Ready For Dev"</param>
        /// <param name="defaultDate">The default date to return if no transition to status is found</param>
        public DateTime DateEnteredStatus(string status, DateTime defaultDate)
        {
            if (this.TransitionHistory == null || this.TransitionHistory.Count == 0) return defaultDate;
            var dateEntered = defaultDate;
            foreach (var item in this.TransitionHistory.Where(t =>
                        t.Type.Equals("Status", StringComparison.OrdinalIgnoreCase) &&
                        t.To.Equals(status, StringComparison.OrdinalIgnoreCase)))
            {
                dateEntered = item.Created;
                break;
            }
            return dateEntered;
        }

        /// <summary>
        /// Finds a particular transition.
        /// </summary>
        /// <param name="fromStatus">e.g. Ready For Dev</param>
        /// <param name="toStatus">e.g. In Analysis</param>
        public TransitionHistoryItem FindStatusTransition(string fromStatus, string toStatus)
        {
            if (this.TransitionHistory == null || this.TransitionHistory.Count == 0) return null;
            return this.TransitionHistory.FirstOrDefault(t => t.Type.Equals("Status", StringComparison.OrdinalIgnoreCase) && t.From.Equals(fromStatus, StringComparison.OrdinalIgnoreCase) && t.To.Equals(toStatus, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Determines the first transition to a given status.
        /// </summary>
        /// <param name="toStatus">e.g. In Play</param>
        public TransitionHistoryItem FindStatusTransitionTo(string toStatus)
        {
            if (this.TransitionHistory == null || this.TransitionHistory.Count == 0) return null;
            return this.TransitionHistory.FirstOrDefault(t => t.To.Equals(toStatus, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Finds a transition set.
        /// </summary>
        /// <param name="fromStatuses">e.g. Ready For Dev, In Play</param>
        /// <param name="toStatus">e.g. In Analysis</param>
        public List<TransitionHistoryItem> FindStatusTransitions(List<string> fromStatuses, string toStatus)
        {
            if (this.TransitionHistory == null || this.TransitionHistory.Count == 0) return null;
            var transitions = new List<TransitionHistoryItem>();
            foreach (var item in this.TransitionHistory.Where(t =>
                       t.Type.Equals("Status", StringComparison.OrdinalIgnoreCase) &&
                       t.To.Equals(toStatus, StringComparison.OrdinalIgnoreCase)))
            {
                transitions.AddRange(from fromStatus in fromStatuses
                                     where item.From.Equals(fromStatus, StringComparison.OrdinalIgnoreCase)
                                     select item);
            }
            return transitions;
        }

        /// <summary>
        /// Determines all transitions to a given status.
        /// </summary>
        /// <param name="toStatus">e.g. In Play</param>
        public List<TransitionHistoryItem> FindStatusTransitionsTo(string toStatus)
        {
            if (this.TransitionHistory == null || this.TransitionHistory.Count == 0) return null;
            var transitions = new List<TransitionHistoryItem>();
            transitions.AddRange(this.TransitionHistory.Where(t =>
                    t.Type.Equals("Status", StringComparison.OrdinalIgnoreCase) &&
                    t.To.Equals(toStatus, StringComparison.OrdinalIgnoreCase)));
            return transitions;
        }

        /// <summary>
        /// Finds the ticket keys where this ticket has been moved in JIRA.
        /// Used to find all SVN commits.
        /// </summary>
        public List<string> AlternateKeys()
        {
            if (this.TransitionHistory == null || this.TransitionHistory.Count == 0) return null;
            return (from transition in this.TransitionHistory where transition.Type.Equals("Key", StringComparison.OrdinalIgnoreCase) select transition.From).ToList();
        }

        public string TaskType
        {
            get { return Issue.Fields.Customfield_11390 != null ? Issue.Fields.Customfield_11390.Value : string.Empty; }
        }

        public List<WorkLogItem> WorkLogs
        {
            get { return Issue.Fields.WorkLog != null ? Issue.Fields.WorkLog.WorkLogs : new List<WorkLogItem>(); }
        }

        public int EstimationSeconds
        {
            get { return Issue.Fields.Timetracking != null ? Issue.Fields.Timetracking.OriginalEstimateSeconds : 0; }
        }

        #region ToString() Overrides for Reporting

        public virtual string ToString(string outputType)
        {
            switch (outputType)
            {
                //case "NoFixVersion":
                //    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", this.Issue.Fields.IssueType.Name,
                //                         this.Key, this.Summary.Replace(",", ""), this.Project, this.Status,
                //                         (string.IsNullOrEmpty(this.Resolution)) ? "Unresolved" : this.Resolution,
                //                         (this.Assignee != null) ? this.Assignee.Name : "Unassigned", this.Reporter.Name);

                //case "DeploymentReport":
                //    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", this.Issue.Fields.IssueType.Name, this.Key,
                //                         this.SummarySummary.Replace(",", ""), this.FixVersion, this.Project,
                //                         this.Status, (this.Assignee != null) ? this.Assignee.Name : "Unassigned",
                //                         this.TestedOn, this.Feedback);

                default:
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", this.Issue.Fields.IssueType.Name, this.Key,
                                         this.Summary.Replace(",", ""), this.Project, this.Status,
                                         (string.IsNullOrEmpty(this.Resolution)) ? "Unresolved" : this.Resolution,
                                         this.FixVersion,
                                         (this.ReleaseDate != DateTime.MinValue)
                                             ? this.ReleaseDate.ToString("dd-MM-yyyy HH:mm:ss")
                                             : string.Empty,
                                         (this.Assignee != null) ? this.Assignee.Name : "Unassigned", this.Reporter.Name);
            }
        }

        public override string ToString()
        {
            return this.ToString(string.Empty);
        }

        #endregion ToString() Overrides for Reporting
    }
}