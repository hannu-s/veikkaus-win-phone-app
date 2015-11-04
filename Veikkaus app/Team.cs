using Newtonsoft.Json;

namespace Veikkaus_app
{
    [JsonObject]
    public class Team
    {
        [JsonProperty]
        public int Id { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public string FullName { get; set; }
        [JsonObject]
        public string Logo { get; set; }
        [JsonObject]
        public string LogoUrl { get; set; }
    }
}
