using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sporttiporssi.Services;
using Sporttiporssi.Models;

namespace Sporttiporssi.ViewModels
{
    public class TradePlayerViewModel : INotifyPropertyChanged
    {
        private readonly LeaguePlayersService _playersService;
        private List<Player> _players { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
