using PrintJiraCards.Models;
using PrintJiraCards.Services.Facade;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace PrintJiraCards.Services
{
    public class PrintGenerator
    {
        public List<Task> GetCards(string jql)
        {
            return Search(new SearchRequest(jql), "*all,-comment", 500);
        }

        public List<Task> Search(SearchRequest query, string fields, int maxResults = 100)
        {
            var tickets = new List<Task>();

            var jiraBaseUrl = ConfigurationManager.AppSettings["JiraBaseUrl"];
            var username = ConfigurationManager.AppSettings["JiraUsername"];
            var password = ConfigurationManager.AppSettings["JiraPassword"];

            var client = new RestClient(jiraBaseUrl) { Authenticator = new HttpBasicAuthenticator(username, password) };
            var request = new RestRequest(query.Url, Method.GET);
            request.AddParameter("jql", query.Jql);
            request.AddParameter("fields", fields);
            request.AddParameter("maxResults", maxResults);
            var response = client.Execute<Search>(request);
            if (response.Data != null) tickets = ParseTasks(response.Data.Issues);
            return tickets;
        }

        private List<Task> ParseTasks(IEnumerable<Issue> issues)
        {
            return issues == null ? null : issues.Select(ParseTask).Where(ticket => ticket != default(Task)).ToList();
        }

        public Task ParseTask(Issue issue)
        {
            if (issue == null || issue.Fields == null) return null;

            var jiraBaseUrl = ConfigurationManager.AppSettings["JiraBaseUrl"];

            var ticket = default(Task);
            switch (issue.Fields.IssueType.Name)
            {
                case "Acceptance Test":
                    ticket = new AcceptanceTest(issue, jiraBaseUrl);
                    break;

                case "Bug":
                    ticket = new Bug(issue, jiraBaseUrl);
                    break;

                case "Defect":
                    ticket = new Defect(issue, jiraBaseUrl);
                    break;

                case "Data Fix":
                    ticket = new DataFix(issue, jiraBaseUrl);
                    break;

                case "Data Migration":
                    ticket = new DataMigration(issue, jiraBaseUrl);
                    break;

                case "Epic":
                    ticket = new Epic(issue, jiraBaseUrl);
                    break;

                case "Change Request":
                    ticket = new ChangeRequest(issue, jiraBaseUrl);
                    break;

                case "Technical Ticket":
                    ticket = new TechnicalTicket(issue, jiraBaseUrl);
                    break;

                case "Spike":
                    ticket = new Spike(issue, jiraBaseUrl);
                    break;

                case "Task":
                    ticket = new StorySubTask(issue, jiraBaseUrl);
                    break;

                default:        // Story
                    ticket = new Story(issue, jiraBaseUrl);
                    break;
            }
            if (ticket.Issue.Fields.SubTasks.Count > 0)
            {
                ticket.SubTasks = ParseSubTasks(ticket.Issue.Fields.SubTasks);
            }
            ticket.TransitionHistory = ParseTransitionHistory(ticket);
            return ticket;
        }

        private static List<SubTask> ParseSubTasks(IEnumerable<Issue> issues)
        {
            return issues == null ? null : issues.Select(ParseSubTask).Where(ticket => ticket != default(SubTask)).ToList();
        }

        private static SubTask ParseSubTask(Issue issue)
        {
            if (issue == null) return null;
            //var ticket = default(SubTask);

            return new SubTask(issue, ConfigurationManager.AppSettings["JiraBaseUrl"]);

            //switch (issue.Fields.IssueType.Name)
            //{
            //    case "Bug":
            //        //ticket = new Bug(issue, ConfigurationManager.AppSettings["JiraBaseUrl"]);
            //        ticket = new SubTask(issue, ConfigurationManager.AppSettings["JiraBaseUrl"]);
            //        break;
            //    case "Acceptance Test":
            //        //ticket = new AcceptanceTest(issue, ConfigurationManager.AppSettings["JiraBaseUrl"]);
            //        ticket = new SubTask(issue, ConfigurationManager.AppSettings["JiraBaseUrl"]);
            //        break;
            //}
            //return ticket;
        }

        private static List<TransitionHistoryItem> ParseTransitionHistory(Task task)
        {
            if (task.Issue.ChangeLog == null) return null;
            var transitions = new List<TransitionHistoryItem>();
            if (task.Issue.ChangeLog.Histories.Count > 0)
            {
                transitions.AddRange(from history in task.Issue.ChangeLog.Histories
                                     from item in history.Items
                                     where item.Field.ToUpper() == "STATUS" || item.Field.ToUpper() == "KEY"
                                     select new TransitionHistoryItem()
                                     {
                                         User = (history.Author != null) ? history.Author.Name : "None",
                                         Type = item.Field.ToUpper(),
                                         Created = history.Created,
                                         From = item.FromString,
                                         To = item.ToString
                                     });
            }
            return transitions;
        }

        public List<Card> GenerateCards(List<Task> tasks)
        {
            var avatarsMap = CreateAvatarMap(tasks);
            return GenerateCards(tasks, avatarsMap);

            //cards.SaveTo("cards.json");

            //DownloadAvatar(avatarsMap.Values);
        }

        private void DownloadAvatar(IEnumerable<Avatar> avatars)
        {
            var jiraBaseUrl = ConfigurationManager.AppSettings["JiraBaseUrl"];
            var username = ConfigurationManager.AppSettings["JiraUsername"];
            var password = ConfigurationManager.AppSettings["JiraPassword"];

            var client = new RestClient(jiraBaseUrl) { Authenticator = new HttpBasicAuthenticator(username, password) };

            foreach (var avatar in avatars)
            {
                if (!File.Exists(avatar.Filename))
                {
                    var request = new RestRequest(avatar.Url, Method.GET);
                    client.DownloadData(request).SaveAs(avatar.Filename);
                }
            }
        }

        private List<Card> GenerateCards(List<Task> tasks, Dictionary<string, Avatar> avatarsMap)
        {
            var cards = new List<Card>();

            var epicsMap = tasks.Where(x => !string.IsNullOrEmpty(x.EpicKey))
                                .ToDictionary(x => x.Key, x => x.EpicKey);

            var epicsSummaries = GetEpicSummaries(epicsMap); //TODO get epic summaries

            foreach (var task in tasks)
            {
                var card = new Card
                {
                    Key = task.Key,
                    ParentKey = task.ParentKey,
                    Assignee = task.AssigneeName,
                    TypeAvatar = avatarsMap[task.IssueTypeObj.IconUrl],
                    UserAvatar = string.IsNullOrEmpty(task.Assignee?.AvatarUrls?.Huge) ? Avatar.Empty : avatarsMap[task.Assignee.AvatarUrls.Huge],
                    Estimated = task.TimeTracking.OriginalEstimate,
                    Remaining = task.TimeTracking.RemainingEstimate,
                    Summary = task.Summary, //task.SummarySnippet
                    HasSubTasks = task.SubTasks?.Count == 0,
                    EpicKey = task.EpicKey ?? string.Empty,
                    IssueType = task.IssueType,
                    Labels = task.Labels
                };

                cards.Add(card);

                if (card.HasParent)
                {
                    card.ParentSummary = task.Parent.Fields.Summary;
                    card.EpicKey = epicsMap.TryGetValue(card.ParentKey, out string parentEpicKey) ? parentEpicKey : string.Empty;
                }

                card.EpicSummary = epicsSummaries.TryGetValue(card.EpicKey, out string epic) ? epic : string.Empty;
            }

            return cards;
        }

        private Dictionary<string, string> GetEpicSummaries(Dictionary<string, string> epicsMap)
        {
            var jql = new StringBuilder();

            foreach (var epicKey in epicsMap.Values.Distinct())
            {
                if (string.IsNullOrEmpty(epicKey)) continue;

                if (jql.Length > 0) jql.Append(" or ");
                jql.Append($"key={epicKey}");
            }

            var summaries = GetCards(jql.ToString()).ToDictionary(c => c.Key, c => c.Summary);

            return summaries;
        }

        private Dictionary<string, Avatar> CreateAvatarMap(List<Task> tasks)
        {
            var avatarsMap = tasks.Select(x => x.IssueTypeObj.IconUrl)
                                  .Distinct()
                                  .Select(x => new Avatar
                                  {
                                      Id = GetAvatarId(x),
                                      Url = x,
                                      Type = "issuetype"
                                  })
                                  .ToDictionary(x => x.Url);

            var usersIcons = tasks.Where(x => !string.IsNullOrEmpty(x.Assignee?.AvatarUrls?.Huge))
                                  .Select(x => x.Assignee.AvatarUrls.Huge)
                                  .Distinct()
                                  .Select(x => new Avatar
                                  {
                                      Id = GetAvatarId(x),
                                      Url = x,
                                      Type = "user"
                                  })
                                  .ToDictionary(x => x.Url);

            foreach (var item in usersIcons) avatarsMap.Add(item.Key, item.Value);
            return avatarsMap;
        }

        private string GetAvatarId(string url)
        {
            var idx = url.IndexOf("avatarId=");
            var endIdx = url.IndexOf("&", idx);

            if (endIdx < 0)
                endIdx = url.Length;

            return url.Substring(idx + 9, endIdx - idx - 9);
        }
    }
}