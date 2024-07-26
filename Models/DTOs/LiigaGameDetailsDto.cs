using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Models.DTOs
{
    public class LiigaGameDetailsDto
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int Season { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TeamDto HomeTeam { get; set; }
        public TeamDto AwayTeam { get; set; }
        public string FinishedType { get; set; }
        public bool Started { get; set; }
        public bool Ended { get; set; }
        public int GameTime { get; set; }
        public int Spectators { get; set; }
        public DateTime CacheUpdateDate { get; set; }
        public bool Stale { get; set; }
        public string Serie { get; set; }
    }
    public class TeamDto
    {
        public string TeamId { get; set; }
        public string TeamPlaceholder { get; set; }
        public string TeamName { get; set; }
        public int Goals { get; set; }
        public string TimeOut { get; set; } // Change type if necessary
        public List<GoalEventDto> GoalEvents { get; set; }
        public int PowerplayInstances { get; set; }
        public int PowerplayGoals { get; set; }
        public int ShortHandedInstances { get; set; }
        public int ShortHandedGoals { get; set; }
        public double ExpectedGoals { get; set; }
        public string Ranking { get; set; } // Change type if necessary
        public DateTime GameStartDateTime { get; set; }
    }

    public class GoalEventDto
    {
        public int ScorerPlayerId { get; set; }
        public DateTime LogTime { get; set; }
        public bool WinningGoal { get; set; }
        public int GameTime { get; set; }
        public int Period { get; set; }
        public int EventId { get; set; }
        public List<string> GoalTypes { get; set; }
        public List<int> AssistantPlayerIds { get; set; }
        public string PlusPlayerIds { get; set; }
        public string MinusPlayerIds { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public Dictionary<int, int> AssistsSoFarInSeason { get; set; }
        public int GoalsSoFarInSeason { get; set; }
        public string VideoClipUrl { get; set; }
        public string VideoThumbnailUrl { get; set; }
    }
}
