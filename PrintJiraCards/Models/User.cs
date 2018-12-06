namespace PrintJiraCards.Models
{
    public class User
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }

        public AvatarUrls AvatarUrls { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}