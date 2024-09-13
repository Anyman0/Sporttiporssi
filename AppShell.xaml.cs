using Sporttiporssi.Views;

namespace Sporttiporssi
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(LeaguesPage), typeof(LeaguesPage));
            Routing.RegisterRoute(nameof(MyTeamPage), typeof(MyTeamPage));
            Routing.RegisterRoute(nameof(TradesPage), typeof(TradesPage));
        }      

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            bool confirmed = await DisplayAlert("Logout", "Are you sure you want to logout?", "OK", "Cancel");
            if(confirmed)
            {
                SecureStorage.Remove("auth_token");
                if (Application.Current is App app)
                {
                    app.NavigateToLoginPage();
                }
            }          
        }

        private void ChooseSerie_Tapped(object sender, TappedEventArgs e)
        {
            if(sender is Label label)
            {
                App.CurrentSerie = label.Text;
                Preferences.Set("currentserie", App.CurrentSerie);
                Preferences.Set("chosen_team", string.Empty);

                ResetLabelStyles();
                label.BackgroundColor = Colors.OrangeRed;
            }
        }

        private void ResetLabelStyles()
        {
            LiigaLabel.BackgroundColor = Colors.Transparent;
            NHLLabel.BackgroundColor = Colors.Transparent;
        }
    }
}
