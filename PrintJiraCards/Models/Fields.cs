using System;
using System.Collections.Generic;

namespace PrintJiraCards.Models
{
    public class Fields
    {
        public string Summary { get; set; }
        public IssueType IssueType { get; set; }
        public User Reporter { get; set; }
        public Status Status { get; set; }
        public JiraProject Project { get; set; }
        public User Assignee { get; set; }
        public Resolution Resolution { get; set; }
        public DateTime Created { get; set; }
        public Comment Comment { get; set; }
        public List<Issue> SubTasks { get; set; }
        public List<Component> Components { get; set; }
        public List<IssueLink> IssueLinks { get; set; }
        public List<string> Labels { get; set; }
        public string Description { get; set; }
        public Parent Parent { get; set; }

        /// <summary>
        /// Planning tab fields
        /// </summary>
        public List<Version> FixVersions { get; set; }          // Fix Version(s)

        public DateTime CustomField_10176 { get; set; }         // Release Date
        public List<string> CustomField_10175 { get; set; }     // Epic/Themes
        public Version CustomField_10990 { get; set; }          // Product Version

        public int TimeEstimate { get; set; }
        public int AggregateTimeOriginalEstimate { get; set; }
        public int TimeOriginalEstimate { get; set; }

        /// <summary>
        /// Story Fields
        /// </summary>
        public string CustomField_10163 { get; set; }           // As A

        public string CustomField_10492 { get; set; }           // When
        public string CustomField_10164 { get; set; }           // I Want
        public string CustomField_10165 { get; set; }           // So That
        public ListItem CustomField_10496 { get; set; }         // Project
        public int CustomField_10177 { get; set; }              // Iteration
        public double CustomField_10030 { get; set; }           // Story Points
        public ListItem CustomField_11092 { get; set; }         // Pod
        public ListItem CustomField_11091 { get; set; }         // Sprint
        public ListItem CustomField_11190 { get; set; }         // Sprint Day

        public string Customfield_10013 { get; set; }

        /// <summary>
        /// Defect fields
        /// </summary>
        public string CustomField_10173 { get; set; }           // Observed

        public string CustomField_10174 { get; set; }           // Expected
        public string CustomField_10172 { get; set; }           // StepsToReproduce
        public ListItem CustomField_10180 { get; set; }         // Environment
        public string CustomField_10167 { get; set; }           // Organisation
        public ListItem CustomField_10490 { get; set; }         // Test Phase
        public ListItem CustomField_10151 { get; set; }         // Severity
        public Version CustomField_10152 { get; set; }          // Found In Version
        public string CustomField_10031 { get; set; }           // Deployment Notes
        public ListItem CustomField_10086 { get; set; }         // Root Cause

        /// <summary>
        /// Acceptance Test fields
        /// </summary>
        public string CustomField_10491 { get; set; }           // Given

        public string CustomField_10493 { get; set; }           // Then

        public ListItem Customfield_11390 { get; set; }           // Then

        public TimeTracking Timetracking { get; set; }           // Then
        public WorkLog WorkLog { get; set; }           // Then

        public Fields()
        {
            this.FixVersions = new List<Version>();
            this.Components = new List<Component>();
            this.SubTasks = new List<Issue>();
            this.IssueLinks = new List<IssueLink>();
            this.CustomField_10175 = new List<string>();
            this.Labels = new List<string>();
        }
    }
}