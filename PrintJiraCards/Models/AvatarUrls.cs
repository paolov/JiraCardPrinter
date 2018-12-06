using RestSharp.Deserializers;

namespace PrintJiraCards.Models
{
    public class AvatarUrls
    {
        //"48x48": "http://jira.afea.eu:8080/secure/useravatar?ownerId=alessandro.ortuso&avatarId=12700",
        //"24x24": "http://jira.afea.eu:8080/secure/useravatar?size=small&ownerId=alessandro.ortuso&avatarId=12700",
        //"16x16": "http://jira.afea.eu:8080/secure/useravatar?size=xsmall&ownerId=alessandro.ortuso&avatarId=12700",
        //"32x32": "http://jira.afea.eu:8080/secure/useravatar?size=medium&ownerId=alessandro.ortuso&avatarId=12700"

        [DeserializeAs(Name = "48x48")]
        public string Huge { get; set; }

        [DeserializeAs(Name = "16x16")]
        public string Small { get; set; }

        [DeserializeAs(Name = "32x32")]
        public string Large { get; set; }

        [DeserializeAs(Name = "24x24")]
        public string Medium { get; set; }
    }
}