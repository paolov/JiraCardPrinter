namespace PrintJiraCards.Models
{
    public class Avatar
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string Filename => Id + (Type == "user" ? ".png" : ".svg");

        public static Avatar Empty
        {
            get
            {
                return new Avatar { };
            }
        }
    }
}