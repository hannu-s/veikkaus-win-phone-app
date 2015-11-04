using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

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

        [JsonProperty]
        public string MatchDate;

        private string GetTeamName(List<Team> team)
        {
            if (team != null || team.Count != 1)
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
            return string.Format("{0} vs {1}", HomeGoals, AwayGoals);
        }

        public string GetMatchDate()
        {
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
            if (team != null || team.Count != 1)
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
    }
}
