using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Veikkaus_app.Common;

namespace Veikkaus_app.JsonObjects
{
    [JsonObject]
    public class Match
    {
        private MatchData MatchData { get; set; }

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

        [JsonProperty]
        public string MatchDate { get; set; }

        public string MatchTitle
        {
            get { return string.Format("{0} vs {1}", GetHomeTeamName(), GetAwayTeamName()); }
        }

        public string MatchResult
        {
            get { return string.Format("{0} - {1}", HomeGoals, AwayGoals); }
        }

        public string MatchShortDate
        {
            get { return GetMatchDate(); }
        }

        private string GetTeamName(List<Team> team)
        {
            if (team != null && team.Count == 1)
                return team[0].Name;
            return string.Empty;
        }

        public string GetAwayTeamName()
        {
            return GetTeamName(AwayTeam);
        }

        public string GetHomeTeamName()
        {
            return GetTeamName(HomeTeam);
        }

        public string GetMatchName()
        {
            return string.Format("{0} vs {1}", GetHomeTeamName(), GetAwayTeamName());
        }

        public string GetMatchResult()
        {
            return string.Format("{0} - {1}", HomeGoals, AwayGoals);
        }

        public string GetMatchDate()
        {
            if (MatchDate.Contains("/"))
                return MatchDate;

            var dateStr = MatchDate.Replace('T', ' ');
            dateStr = dateStr.Replace('Z', ' ');
            return DateTime.ParseExact(dateStr, "yyyy-MM-dd HH:mm:ss ", CultureInfo.CurrentCulture).ToShortDateString();
        }

        public string GetMatchId()
        {
            return Id.ToString();
        }

        private string GetLogoUrl(List<Team> team)
        {
            if (team != null && team.Count == 1)
                return team[0].LogoUrl;
            return string.Empty;
        }

        public Uri GetHomeTeamLogoUri()
        {
            return new Uri(GetLogoUrl(HomeTeam));
        }

        public Uri GetAwayTeamLogoUri()
        {
            return new Uri(GetLogoUrl(AwayTeam));
        }

        public async Task<MatchData> GetMatchDataAsync()
        {
            if (MatchData != null)
                return MatchData;

            var client = new AppHttpClient();

            var dataString = await client.GetMatchDataAsync(GetMatchId());
            MatchData = JsonMatchDeserializer.GetMatchDataFromJsonString(dataString);

            return MatchData;
        }
    }
}
