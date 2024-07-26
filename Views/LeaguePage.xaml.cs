namespace Sporttiporssi.Views;
using Sporttiporssi.ViewModels;

public partial class LeaguePage : ContentPage
{
    private LeagueViewModel _leagueViewModel;
	public LeaguePage(LeagueViewModel leagueViewModel)
	{
        Shell.SetNavBarIsVisible(this, false);
        InitializeComponent();
        _leagueViewModel = leagueViewModel;
        BindingContext = _leagueViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _leagueViewModel.LoadStandings();
    }
}