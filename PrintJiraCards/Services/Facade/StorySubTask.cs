using PrintJiraCards.Models;

namespace PrintJiraCards.Services.Facade
{
    public class StorySubTask : Task
    {
        public StorySubTask(Issue issue, string jiraUrl)
            : base(issue, jiraUrl)
        {
            Description = string.Format("Task type: {0}", TaskType);
        }

        public string Description { get; set; }

        public double Points => 0;

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
    }
}