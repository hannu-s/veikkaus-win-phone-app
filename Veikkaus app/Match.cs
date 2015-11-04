using Newtonsoft.Json;
using System.Collections.Generic;

namespace Veikkaus_app
{
    [JsonObject]
    public class Match
    {
        [JsonProperty]
        public int Id { get; set; }
        [JsonProperty]
        public int HomeGoals { get; set; }
        [JsonProperty]
        public int AwayGoals { get; set; }
        [JsonProperty]
        [JsonConverter(typeof(ObjectToListConverter<Team>))]
        public List<Team> HomeTeam { get; set; }
        [JsonProperty]
        [JsonConverter(typeof(ObjectToListConverter<Team>))]
        public List<Team> AwayTeam { get; set; }

    }
}
