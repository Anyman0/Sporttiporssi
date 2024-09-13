using CommunityToolkit.Maui.Views;
using Sporttiporssi.Views.Popups;
using Sporttiporssi.ViewModels;
using System.Diagnostics;
using Sporttiporssi.Models;
using Sporttiporssi.Models.DTOs;
using System.Globalization;

namespace Sporttiporssi.Views
{
    public partial class MainPage : ContentPage
    {
        private GamesViewModel _gamesViewModel;
        private readonly IServiceProvider _serviceProvider;
        private DateTime gameDate;
        public MainPage(GamesViewModel gamesViewModel, IServiceProvider serviceProvider)
        {
            //Shell.SetNavBarIsVisible(this, false);
            InitializeComponent();

            _gamesViewModel = gamesViewModel;
            BindingContext = _gamesViewModel;
            _serviceProvider = serviceProvider;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //gameDate = DateTime.Now.Date;
            var gameDateString = Preferences.Get("chosenDate", string.Empty);
            if(string.IsNullOrEmpty(gameDateString)) 
            {
                gameDate = DateTime.Now.Date;
                Preferences.Set("chosenDate", gameDate.ToString());
            }
            else
            {
                gameDate = DateTime.Parse(gameDateString);
            }            
            GamesLabel.Text = $"Ottelut {gameDate.ToString("dd.M.yyyy")}";
            await GetGamesByDate();
        }

        private async Task GetGamesByDate()
        {
            await _gamesViewModel.LoadGamesByDate(gameDate);            
        }
        
        protected override void OnDisappearing()
        {
            base.OnDisappearing();          
        }
        
        private void Stats_Tapped(object sender, TappedEventArgs e)
        {
            Debug.WriteLine("Stats tapped!");
            var image = sender as Image;
            var game = image.BindingContext as Game;
            
            if (game != null)
            {
                if(game.IsRosterExpanded)
                {
                    _gamesViewModel.ToggleRosterExpansion(game);
                }
                //_gamesViewModel.ToggleStatsExpansion(game);
            }
        }

        private async void Roster_Tapped(object sender, TappedEventArgs e)
        {
            Debug.WriteLine("Roster tapped!");
            var image = sender as Image;
            var game = image.BindingContext as Game;
            var rank = game.HomeTeamRanking;
            if (game != null) 
            { 
                if(game.IsStatsExpanded)
                {
                    //_gamesViewModel.ToggleStatsExpansion(game);
                }
                else
                {
                    _gamesViewModel.ToggleRosterExpansion(game);
                }
                if (game.IsRosterExpanded)
                {
                    //Load labels with rosterdata
                    await _gamesViewModel.GetGameStats(game.HomeTeamName, game.AwayTeamName, game.Start);
                }
            }
        }

        private async void RightArrow_Tapped(object sender, TappedEventArgs e)
        {
            gameDate = gameDate.AddDays(1);
            await GetGamesByDate();
            Preferences.Set("chosenDate", gameDate.ToString());
            GamesLabel.Text = $"Ottelut {gameDate.ToString("dd.M.yyyy")}";
        }

        private async void LeftArrow_Tapped(object sender, TappedEventArgs e)
        {
            gameDate = gameDate.AddDays(-1);
            await GetGamesByDate();
            Preferences.Set("chosenDate", gameDate.ToString());
            GamesLabel.Text = $"Ottelut {gameDate.ToString("dd.M.yyyy")}";
        }

        private async void LogoutToolbarItem_Clicked(object sender, EventArgs e)
        {
            bool confirmed = await DisplayAlert("Logout", "Are you sure you want to logout?", "OK", "Cancel");
            if (confirmed)
            {
                SecureStorage.Remove("auth_token");
                if (Application.Current is App app)
                {
                    app.NavigateToLoginPage();
                }
            }
        }
    }

}
