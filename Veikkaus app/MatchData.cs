using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Veikkaus_app
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
