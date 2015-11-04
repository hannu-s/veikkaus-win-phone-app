using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Veikkaus_app.JsonObjects;

namespace Veikkaus_app.Common
{
    public class JsonMatchDeserializer
    {
        public static List<Match> GetMatchListFromJsonString(string jsonData)
        {
            var result = new List<Match>();
            try
            {
                result = JsonConvert.DeserializeObject<List<Match>>(jsonData, new ObjectToListConverter<Team>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }

        public static MatchData GetMatchDataFromJsonString(string jsonData)
        {
            var result = new MatchData();
            try
            {
                result = JsonConvert.DeserializeObject<MatchData>(jsonData, new ObjectToArrayConverter<List<Match>>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }
    }
}
