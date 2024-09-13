using Sporttiporssi.Models;
using Sporttiporssi.ViewModels;

namespace Sporttiporssi.Views;

public partial class ChangeTeamPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TeamViewModel _teamViewModel;
    public ChangeTeamPage(IServiceProvider serviceProvider, TeamViewModel teamViewModel)
	{
		InitializeComponent();
        _serviceProvider = serviceProvider;
        _teamViewModel = teamViewModel;
        BindingContext = _teamViewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        LoadUserTeams();
    }
    private async void LoadUserTeams()
    {
        var chosenTeam = await _teamViewModel.LoadAllUserTeams();
        if(chosenTeam != null)
        {
            TeamListView.SelectedItem = chosenTeam;
        }
    }

    private async void CreateTeamToolbarItem_Clicked(object sender, EventArgs e)
    {
        var createTeamPage = _serviceProvider.GetRequiredService<CreateTeamPage>();
        await Shell.Current.Navigation.PushAsync(createTeamPage);
    }

    private void TeamListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var currentTeam = e.Item as FantasyTeam;
        if(currentTeam != null)
        {
            Preferences.Set("chosen_team", currentTeam.FantasyTeamId.ToString());
            TeamListView.SelectedItem = currentTeam;
        }       
    }
}