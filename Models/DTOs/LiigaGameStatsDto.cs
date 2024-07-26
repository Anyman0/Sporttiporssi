using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Models.DTOs
{
    public class LiigaGameStatsDto
    {
        public GameTeam Game { get; set; }
        public List<Award> Awards { get; set; }
        public ObservableCollection<GameStatsPlayer> HomeTeamPlayers { get; set; }
        public ObservableCollection<GameStatsPlayer> AwayTeamPlayers { get; set; }
    }

    public class GameTeam
    {
        public int Id { get; set; }
        public int Season { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        // Add other properties as per your JSON schema
    }

    public class Award
    {
        public int Id { get; set; }
        public int AwardCategory { get; set; }
        public string AwardIssuer { get; set; }
        public string AwardName { get; set; }
        public int AwardPoint { get; set; }
        public int PlayerId { get; set; }
        public string TeamId { get; set; }
    }

    public class GameStatsPlayer
    {
        public int Id { get; set; }
        public string TeamId { get; set; }
        public string TeamName { get; set; }
        public int? Line { get; set; }
        public string CountryOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string RoleCode { get; set; }
        public string Handedness { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public bool Captain { get; set; }
        public bool Rookie { get; set; }
        public bool AlternateCaptain { get; set; }
        public int Jersey { get; set; }
        public string PictureUrl { get; set; }
        public bool Injured { get; set; }
        public bool Suspended { get; set; }
        public bool Removed { get; set; }
        // Add other properties as per your JSON schema
    }
}
