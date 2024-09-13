using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Models
{
    public class FantasyTeam : INotifyPropertyChanged
    {
        public Guid FantasyTeamId { get; set; } // Primary key
        public string TeamName { get; set; }
        public Guid Serie { get; set; }
        public int UserId { get; set; }
        public int TradesThisPhase { get; set; }
        public int FundsLeft { get; set; }
        public int TotalFTP {  get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime CreatedAt { get; set; }
        private int _teamValue { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int TeamValue
        {
            get => _teamValue;
            set
            {
                _teamValue = value;
                OnPropertyChanged(nameof(TeamValue));
            }
        }

    }
}
