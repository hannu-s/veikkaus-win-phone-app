using Newtonsoft.Json;

namespace Veikkaus_app.JsonObjects
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
        [JsonProperty]
        public string Logo { get; set; }
        [JsonProperty]
        public string LogoUrl { get; set; }
    }
}
