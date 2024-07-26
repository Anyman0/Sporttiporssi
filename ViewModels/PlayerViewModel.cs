using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sporttiporssi.Services;
using Sporttiporssi.Models;
using System.ComponentModel;

namespace Sporttiporssi.ViewModels
{
    public class PlayerViewModel : INotifyPropertyChanged
    {
        private readonly LeaguePlayersService _playersService;
        private List<Player> _players { get; set; }
        private readonly string Liiga_baseApiUrl = "https://www.liiga.fi/api/v2/";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<Player> Players
        {
            get => _players;
            set
            {
                _players = value;
                OnPropertyChanged(nameof(Players));
            }
        }

        public PlayerViewModel(LeaguePlayersService playersService)
        {
            _playersService = playersService;
            LoadAllPlayersAsync();
        }

        private async Task LoadAllPlayersAsync()
        {
            //Players = await _playersService.GetAllPlayersAsync();
            Players = await _playersService.GetPlayersAsync();
        }
    }
}
