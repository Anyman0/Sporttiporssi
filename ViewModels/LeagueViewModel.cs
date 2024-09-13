using Sporttiporssi.Models;
using Sporttiporssi.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sporttiporssi.Services;

namespace Sporttiporssi.ViewModels
{
    public class LeagueViewModel : INotifyPropertyChanged
    {
        
        private readonly LeagueStandingsService _leagueStandingsService;
        private ObservableCollection<LeagueStanding> _standings;

        public ObservableCollection<LeagueStanding> LeagueStandings
        {
            get => _standings;
            set
            {
                _standings = value;
                OnPropertyChanged(nameof(LeagueStandings));
            }
        }
        public LeagueViewModel(LeagueStandingsService leagueStandingsService)
        {
            _leagueStandingsService = leagueStandingsService;                      
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
   
        public async void LoadStandings()
        {
            LeagueStandings = await _leagueStandingsService.GetLeagueStandings();
        }
             
    }
}
