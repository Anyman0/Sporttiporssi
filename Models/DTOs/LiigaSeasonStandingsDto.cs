using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Models.DTOs
{

    public class LiigaSeasonStandingsResponse
    {
        [JsonProperty("season")]
        public List<LiigaSeasonStandingsDto> SeasonStandings { get; set; }
    }
    public class LiigaSeasonStandingsDto
    {
        [JsonProperty("internalId")]
        public int InternalId { get; set; }

        [JsonProperty("teamId")]
        public string TeamId { get; set; }

        public string TeamName
        {
            get
            {
                var teamName = TeamId.Split(':')[1];
                return teamName.Length < 4 ? teamName.ToUpper() : char.ToUpper(teamName[0]) + teamName.Substring(1).ToLower();
            }
        }

        [JsonProperty("ranking")]
        public int Ranking { get; set; }

        [JsonProperty("games")]
        public int Games { get; set; }

        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("winPercentage")]
        public string WinPercentage { get; set; }

        [JsonProperty("overtimeWins")]
        public int OvertimeWins { get; set; }

        [JsonProperty("losses")]
        public int Losses { get; set; }

        [JsonProperty("overtimeLosses")]
        public int OvertimeLosses { get; set; }

        [JsonProperty("ties")]
        public int Ties { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        [JsonProperty("goals")]
        public int Goals { get; set; }

        [JsonProperty("goalsAgainst")]
        public int GoalsAgainst { get; set; }

        [JsonProperty("powerPlayPercentage")]
        public string PowerPlayPercentage { get; set; }

        [JsonProperty("powerPlayInstances")]
        public int PowerPlayInstances { get; set; }

        [JsonProperty("powerPlayTime")]
        public int PowerPlayTime { get; set; }

        [JsonProperty("powerPlayGoals")]
        public int PowerPlayGoals { get; set; }

        [JsonProperty("shortHandedPercentage")]
        public string ShortHandedPercentage { get; set; }

        [JsonProperty("shortHandedInstances")]
        public int ShortHandedInstances { get; set; }

        [JsonProperty("shortHandedTime")]
        public int ShortHandedTime { get; set; }

        [JsonProperty("shortHandedGoalsAgainst")]
        public int ShortHandedGoalsAgainst { get; set; }

        [JsonProperty("penaltyMinutes")]
        public int PenaltyMinutes { get; set; }

        [JsonProperty("twoMinutePenalties")]
        public int TwoMinutePenalties { get; set; }

        [JsonProperty("fiveMinutePenalties")]
        public int FiveMinutePenalties { get; set; }

        [JsonProperty("tenMinutePenalties")]
        public int TenMinutePenalties { get; set; }

        [JsonProperty("twentyMinutePenalties")]
        public int TwentyMinutePenalties { get; set; }

        [JsonProperty("twentyFiveMinutePenalties")]
        public int TwentyFiveMinutePenalties { get; set; }

        [JsonProperty("totalPenalties")]
        public int TotalPenalties { get; set; }

        [JsonProperty("liveRanking")]
        public int LiveRanking { get; set; }

        [JsonProperty("liveGames")]
        public int LiveGames { get; set; }

        [JsonProperty("liveWins")]
        public int LiveWins { get; set; }

        [JsonProperty("liveLosses")]
        public int LiveLosses { get; set; }

        [JsonProperty("liveTies")]
        public int LiveTies { get; set; }

        [JsonProperty("livePoints")]
        public int LivePoints { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("distancePerGame")]
        public double DistancePerGame { get; set; }

        private double _pointsPerGame;

        [JsonProperty("pointsPerGame")]
        public double PointsPerGame
        {
            get => _pointsPerGame;
            set => _pointsPerGame = Math.Round(value, 2); // Round to two decimal places
        }
    }
}
