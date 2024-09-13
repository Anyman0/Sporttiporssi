using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Models
{
    public class Player :  INotifyPropertyChanged
    {
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
        public string? TimeOnIceAvg { get; set; }
        public double? FaceoffWonPercentage { get; set; }
        public double? ShotPercentage { get; set; }
        public int? FaceoffsTotal { get; set; }
        public DateTime LastUpdated { get; set; }
        public int? FTP {  get; set; }
        public int Price { get; set; }
        public string? Name => $"{FirstName[0]}.{LastName}";
        public string? FormattedPrice => $"{Price / 1000}k";

        public string DisplayGoals => Goals.HasValue ? Goals.Value.ToString() : "-";
        public string DisplayAssists => Assists.HasValue ? Assists.Value.ToString() : "-";
        public string DisplayPoints => Points.HasValue ? Points.Value.ToString() : "-";
        public string DisplayShots => Shots.HasValue ? Shots.Value.ToString() : "-";
        private bool _isSold;
        public bool IsSold
        {
            get => _isSold;
            set
            {
                if (_isSold != value)
                {
                    _isSold = value;
                    OnPropertyChanged(nameof(IsSold));   
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
