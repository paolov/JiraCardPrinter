using System.Collections.Generic;

namespace PrintJiraCards.Models
{
    public class Card
    {
        public string Key { get; set; }
        public string ParentKey { get; set; }
        public string Assignee { get; set; }
        public Avatar TypeAvatar { get; set; }
        public Avatar UserAvatar { get; set; }
        public string Estimated { get; set; }
        public string Remaining { get; set; }
        public string Summary { get; set; }
        public bool HasSubTasks { get; set; }
        public string ParentSummary { get; set; }
        public string EpicKey { get; set; }
        public string EpicSummary { get; set; }
        public string IssueType { get; set; } //IssueType = { "Bug", "Sub-task", "Task""Story" }

        public bool HasParent => !string.IsNullOrEmpty(ParentKey);
        public bool HasEpic => !string.IsNullOrEmpty(EpicSummary) || !string.IsNullOrEmpty(EpicKey);

        public string FullKey => HasParent ? $"{ParentKey}\\{Key}" : Key;
        public string EstimateTotal => string.IsNullOrEmpty(Remaining) || Remaining == "0m" ? Estimated : Remaining;
        public string Epic => string.IsNullOrEmpty(EpicSummary) ? EpicKey : EpicSummary;

        public List<string> Labels { get; internal set; }
    }
}