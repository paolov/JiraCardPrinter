namespace PrintJiraCards.Services
{
    public class SearchRequest
    {
        public SearchRequest()
        {
            Url = "/rest/api/2/search";
        }

        public SearchRequest(string jql) : this()
        {
            Jql = jql;
        }

        public string Url { get; set; }
        public string Jql { get; set; }
    }
}