using Newtonsoft.Json;
using System.Collections.Generic;
using Veikkaus_app.Common;

namespace Veikkaus_app.JsonObjects
{
    [JsonObject]
    public class MatchData
    {
        [JsonProperty]
        [JsonConverter(typeof(ObjectToArrayConverter<Match>))]
        public List<Match> Match { get; set; }

        public Match GetMatch()
        {
            return (Match != null && Match.Count == 1) ? Match[0] : null;
        }
    }
}
