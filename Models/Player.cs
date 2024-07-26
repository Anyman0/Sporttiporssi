using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Models
{
    public class Player
    {
        [PrimaryKey]
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public string? TeamName { get; set; }
        public string? TeamShortName { get; set; }
        public string? Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Nationality { get; set; }
        public string? Tournament { get; set; }
        public string? PictureUrl { get; set; }
        public string? PreviousTeamsForTournament { get; set; }
        public bool Injured { get; set; }
        public int Jersey { get; set; }
        public int LastSeason { get; set; }
        public bool Goalkeeper { get; set; }
        public int Games { get; set; }
        public int PlayedGames { get; set; }
        public bool Rookie { get; set; }
        public bool Suspended { get; set; }
        public bool Removed { get; set; }
        public int? TimeOnIce { get; set; }
        public bool Current { get; set; }
        public int? Goals { get; set; }
        public int? Assists { get; set; }
        public int? Points { get; set; }
        public int? Plus { get; set; }
        public int? Minus { get; set; }
        public int? PlusMinus { get; set; }
        public int? PenaltyMinutes { get; set; }
        public int? PowerplayGoals { get; set; }
        public int? PenaltykillGoals { get; set; }
        public int WinningGoals { get; set; }
        public int? Shots { get; set; }
        public int? ShotsIntoGoal { get; set; }
        public int? FaceoffsWon { get; set; }
        public int? FaceoffsLost { get; set; }
        public double? ExpectedGoals { get; set; }
        public int? TimeOnIceAvg { get; set; }
        public double? FaceoffWonPercentage { get; set; }
        public double? ShotPercentage { get; set; }
        public int? FaceoffsTotal { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
