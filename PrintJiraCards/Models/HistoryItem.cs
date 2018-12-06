namespace PrintJiraCards.Models
{
    public class HistoryItem
    {
        public string Field { get; set; }
        public string FieldType { get; set; }
        public string FromString { get; set; }
        public new string ToString { get; set; }
    }
}